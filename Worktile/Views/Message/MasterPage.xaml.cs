using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Windows.UI.Xaml.Input;
using Worktile.Views.Message.Dialog;
using Windows.System;
using Worktile.ViewModels;
using Worktile.ViewModels.Message;
using Worktile.Models.Message;

namespace Worktile.Views.Message
{
    public sealed partial class MasterPage : Page, INotifyPropertyChanged
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MasterViewModel _viewModel;
        private MasterViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var mainViewModel = e.Parameter as MainViewModel;
            ViewModel = new MasterViewModel(mainViewModel, ContentFrame);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.Dispose();
        }



        private Session _rightTappedSession;

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            ListViewItemMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            _rightTappedSession = ((FrameworkElement)e.OriginalSource).DataContext as Session;
            if (_rightTappedSession.Starred)
            {
                ViewModel.StarVisibility = Visibility.Collapsed;
                ViewModel.UnStarVisibility = Visibility.Visible;
            }
            else
            {
                ViewModel.StarVisibility = Visibility.Visible;
                ViewModel.UnStarVisibility = Visibility.Collapsed;
            }
        }

        private async void CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateGroupDialog();
            dialog.OnCreateSuccess += channel =>
            {
                var session = MessageHelper.GetSession(channel);
                ViewModel.Sessions.Insert(0, session);
            };
            await dialog.ShowAsync();
        }

        private void CreateNewSession(Session session)
        {
            var ss = ViewModel.Sessions.SingleOrDefault(s => s.Id == session.Id);
            if (ss == null)
            {
                ViewModel.Sessions.Insert(0, session);
                ViewModel.SelectedSession = session;
            }
            else if (ViewModel.SelectedSession != ss)
            {
                ViewModel.Sessions.Remove(ss);
                ViewModel.Sessions.Insert(0, ss);
                ViewModel.SelectedSession = ss;
            }
        }

        private async void JoinGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new JoinGroupDialog();
            dialog.OnActived += session => CreateNewSession(session);
            dialog.OnJoined += session =>
            {
                ViewModel.Sessions.Insert(0, session);
                ViewModel.SelectedSession = session;
            };
            await dialog.ShowAsync();
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain + "/console/members?add=true"));
        }

        private async void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateChatDialog();
            dialog.OnSessionCreated += session => CreateNewSession(session);
            await dialog.ShowAsync();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DeleteSessionAsync(_rightTappedSession);
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.StarSessionAsync(_rightTappedSession);
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UnStarSessionAsync(_rightTappedSession);
        }
    }
}
