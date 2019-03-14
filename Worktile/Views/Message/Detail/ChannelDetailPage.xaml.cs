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
using Worktile.Enums;
using Worktile.Common.Communication;
using Worktile.Operators.Message.Detail;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class ChannelDetailPage : Page, INotifyPropertyChanged
    {
        public ChannelDetailPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            ChannelDetailOperator.ContentFrame = ContentFrame;
            var session = e.Parameter as ChannelSession;
            ViewModel = new ChannelDetailViewModel(session);
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
            ContentFrame.Navigate(typeof(AddMemberPage), ViewModel.Session);
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

        private async void ExitChannel_Click(object sender, RoutedEventArgs e)
        {
            string content = ViewModel.Session.Visibility == WtVisibility.Public
                ? "该群组为公开群组，退出群组后，你将收不到该群组中的消息，但是仍然可以搜索到群组消息。稍后你可以从群组列表中重新加入该群组。确定要退出群组吗？"
                : "该群组为私有群组，退出群组后，你将收不到该群组中的消息，并且无法重新加入，除非该群组中的成员邀请你加入。确定要退出群组吗？";
            var dialog = new ContentDialog
            {
                Title = "退出群组",
                Content = content,
                PrimaryButtonText = "取消",
                SecondaryButtonText = "确认退出",
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                await ViewModel.ExitChannelAsync();
            }
        }
    }
}
