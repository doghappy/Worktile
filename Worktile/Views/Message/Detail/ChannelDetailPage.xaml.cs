using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using Worktile.Models.Member;
using Windows.System;
using System;
using Worktile.Common;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class ChannelDetailPage : Page, INotifyPropertyChanged
    {
        public ChannelDetailPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ToMessageDetailPageParam _navParam;

        private ChannelDetailViewModel _viewModel;
        private ChannelDetailViewModel ViewModel
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navParam = e.Parameter as ToMessageDetailPageParam;
            var session = _navParam.Session as ChannelSession;
            ViewModel = new ChannelDetailViewModel(session, ContentFrame, _navParam.MainViewModel);
        }

        private void ContactInfo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsPaneOpen = true;
            ViewModel.LoadMembersAvatar();
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            var position = e.GetPosition(listView);
            position.X = -2;
            MemberCard.Member = ((FrameworkElement)e.OriginalSource).DataContext as Member;
            MemberCardFlyout.ShowAt(listView, new FlyoutShowOptions
            {
                Position = position,
                Placement = FlyoutPlacementMode.Left
            });
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.StarSessionAsync(ViewModel.Session);
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UnStarSessionAsync(ViewModel.Session);
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(DataSource.SubDomain + "/console/services");
            await Launcher.LaunchUriAsync(uri);
        }

        private void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(AddMemberPage));
            Nav.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
            Nav.BackRequested += Nav_BackRequested;
            Nav.IsBackEnabled = true;
        }

        private void Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            ContentFrame.GoBack();
            Nav.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            Nav.BackRequested -= Nav_BackRequested;
            Nav.IsBackEnabled = false;
        }
    }
}
