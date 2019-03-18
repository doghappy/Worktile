using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Windows.UI.Xaml.Input;
using Windows.System;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail;
using Worktile.Operators;
using System.Threading.Tasks;
using Worktile.Services;
using System.Collections.Generic;
using Worktile.Enums;
using System.Collections.ObjectModel;
using Worktile.Enums.Privileges;

namespace Worktile.Views.Message
{
    public sealed partial class MasterPage : Page, INotifyPropertyChanged
    {
        public MasterPage()
        {
            InitializeComponent();
            _teamService = new TeamService();
            _messageService = new MessageService();
            Sessions = new ObservableCollection<ISession>();
            SetPermission();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly TeamService _teamService;
        readonly MessageService _messageService;
        LightMainPage _mainPage;

        public ObservableCollection<ISession> Sessions { get; }
        public bool CanAddMember { get; private set; }

        private Visibility _starVisibility;
        public Visibility StarVisibility
        {
            get => _starVisibility;
            set
            {
                if (_starVisibility != value)
                {
                    _starVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StarVisibility)));
                }
            }
        }

        private Visibility _unStarVisibility;
        public Visibility UnStarVisibility
        {
            get => _unStarVisibility;
            set
            {
                if (_unStarVisibility != value)
                {
                    _unStarVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnStarVisibility)));
                }
            }
        }

        private ISession _selectedSession;
        public ISession SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (_selectedSession != value)
                {
                    _selectedSession = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSession)));
                }
            }
        }

        private void SetPermission()
        {
            int messagePermission = Convert.ToInt32(DataSource.Team.Privileges.Message.Value, 2);
            CanAddMember = ((int)AdminPrivilege.Member & messagePermission) != 0;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //MasterOperator.ContentFrame = MasterContentFrame;
            _mainPage = this.GetParent<LightMainPage>();
            await LoadChatAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //ViewModel.Dispose();
        }

        private async Task LoadChatAsync()
        {
            _mainPage.IsActive = true;
            var data = await _teamService.GetChatAsync();
            var list = new List<ISession>();
            bool flag = !DataSource.JoinedChannels.Any();
            var channels = data.Channels.Concat(data.Groups);
            foreach (var item in channels)
            {
                item.TethysAvatar = AvatarHelper.GetAvatar(item);
                list.Add(item);
                if (flag)
                {
                    DataSource.JoinedChannels.Add(item as ChannelSession);
                }
            }

            foreach (var item in data.Sessions)
            {
                item.NamePinyin = item.To.DisplayNamePinyin;
                item.TethysAvatar = AvatarHelper.GetAvatar(item, AvatarSize.X80);
                list.Add(item);
            }
            list.Sort((a, b) =>
            {
                if (a.Starred && !b.Starred)
                    return -1;
                if (!a.Starred && b.Starred)
                    return 1;
                if (a.LatestMessageAt > b.LatestMessageAt)
                    return -1;
                if (a.LatestMessageAt < b.LatestMessageAt)
                    return 1;
                if (a.Show > 0 && b.Show <= 0)
                    return -1;
                if (a.Show <= 0 && b.Show > 0)
                    return 1;
                if (a.UnRead > 0 && b.UnRead <= 0)
                    return -1;
                if (a.UnRead <= 0 && b.UnRead > 0)
                    return 1;
                return a.NamePinyin.CompareTo(b.NamePinyin);
            });
            foreach (var item in list)
            {
                Sessions.Add(item);
            }
            _mainPage.IsActive = false;
        }

        private ISession _rightTappedSession;

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            ListViewItemMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            _rightTappedSession = ((FrameworkElement)e.OriginalSource).DataContext as ISession;
            if (_rightTappedSession.Starred)
            {
                StarVisibility = Visibility.Collapsed;
                UnStarVisibility = Visibility.Visible;
            }
            else
            {
                StarVisibility = Visibility.Visible;
                UnStarVisibility = Visibility.Collapsed;
            }
        }

        private void CreateChannel_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(CreateChannelPage);
            //EnableNavGoBack(sourcePageType);
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            var names = new[] { nameof(TransparentPage), nameof(CreateChannelPage), nameof(JoinChannelPage), nameof(CreateChatPage) };
            while (true)
            {
                var item = MasterContentFrame.BackStack.FirstOrDefault(t => names.Contains(t.SourcePageType.Name));
                if (item == null)
                {
                    break;
                }
                else
                {
                    MasterContentFrame.BackStack.Remove(item);
                }
            }

            if (MasterContentFrame.CanGoBack)
            {
                MasterContentFrame.GoBack();
            }
            else
            {
                MasterContentFrame.Navigate(typeof(TransparentPage));
            }
            MainOperator.NavView.IsBackEnabled = false;
            MainOperator.NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            MainOperator.NavView.BackRequested -= NavigationView_BackRequested;
        }

        private void CreateNewSession(ISession session)
        {
            var ss = Sessions.SingleOrDefault(s => s.Id == session.Id);
            if (ss == null)
            {
                Sessions.Insert(0, session);
                SelectedSession = session;
            }
            else if (SelectedSession != ss)
            {
                Sessions.Remove(ss);
                Sessions.Insert(0, ss);
                SelectedSession = ss;
            }
        }

        private void JoinChannel_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(JoinChannelPage);
            //EnableNavGoBack(sourcePageType);
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain + "/console/members?add=true"));
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(CreateChatPage);
            //EnableNavGoBack(sourcePageType);
        }

        //private void EnableNavGoBack(Type sourcePageType)
        //{
        //    if (MasterContentFrame.CurrentSourcePageType != sourcePageType)
        //    {
        //        MasterContentFrame.Navigate(sourcePageType, ViewModel);
        //        MainOperator.NavView.IsBackEnabled = true;
        //        MainOperator.NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
        //        MainOperator.NavView.BackRequested += NavigationView_BackRequested;
        //    }
        //}

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.DeleteSessionAsync(_rightTappedSession.Id);
            if (result)
            {
                Sessions.Remove(_rightTappedSession);
            }
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.StarSessionAsync(_rightTappedSession);
            if (result)
            {
                _rightTappedSession.Starred = true;
            }
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.UnStarSessionAsync(_rightTappedSession);
            if (result)
            {
                _rightTappedSession.Starred = false;
            }
        }
    }
}
