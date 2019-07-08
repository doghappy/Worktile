using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Message.Details
{
    public sealed partial class UnreadListPage : Page
    {
        public UnreadListPage()
        {
            InitializeComponent();
            ViewModel = new UnreadListViewModel();
        }

        public UnreadListViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
        }

        long _ticks = 0;

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            long ticks = DateTime.Now.Ticks;
            if (ViewModel.HasMore
                && scrollViewer.VerticalOffset <= 10
                && (_ticks == 0 || new TimeSpan(ticks - _ticks).TotalSeconds > .5))
            {
                _ticks = ticks;
                await ViewModel.LoadMessagesAsync();
            }
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as WtMessage.Message;
            await ViewModel.PinAsync(msg);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as WtMessage.Message;
            await ViewModel.UnPinAsync(msg);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadMessagesAsync();
        }
    }
}
