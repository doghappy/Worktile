using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Windows.UI.Xaml.Input;
using Windows.System;
using Worktile.ViewModels;
using Worktile.ViewModels.Message;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail;

namespace Worktile.Views.Message
{
    public sealed partial class MasterPage : Page, INotifyPropertyChanged
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        NavigationView _mainNavView;

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
            _mainNavView = this.GetParent<NavigationView>("MainNavView");
            await ViewModel.InitializeAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var mainViewModel = e.Parameter as MainViewModel;
            ViewModel = new MasterViewModel
            {
                ContentFrame = MasterContentFrame,
                MainViewModel = mainViewModel
            };
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.Dispose();
        }

        private ISession _rightTappedSession;

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            ListViewItemMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            _rightTappedSession = ((FrameworkElement)e.OriginalSource).DataContext as ISession;
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

        private void CreateChannel_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(CreateChannelPage);
            EnableNavGoBack(sourcePageType);
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
            _mainNavView.IsBackEnabled = false;
            _mainNavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            _mainNavView.BackRequested -= NavigationView_BackRequested;
        }

        private void CreateNewSession(ISession session)
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

        private void JoinChannel_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(JoinChannelPage);
            EnableNavGoBack(sourcePageType);
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain + "/console/members?add=true"));
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(CreateChatPage);
            EnableNavGoBack(sourcePageType);
        }

        private void EnableNavGoBack(Type sourcePageType)
        {
            if (MasterContentFrame.CurrentSourcePageType != sourcePageType)
            {
                MasterContentFrame.Navigate(sourcePageType, ViewModel);
                _mainNavView.IsBackEnabled = true;
                _mainNavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
                _mainNavView.BackRequested += NavigationView_BackRequested;
            }
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
