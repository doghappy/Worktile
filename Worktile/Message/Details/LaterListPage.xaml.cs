using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Models;

namespace Worktile.Message.Details
{
    public sealed partial class LaterListPage : Page
    {
        public LaterListPage()
        {
            InitializeComponent();
            ViewModel = new LaterListViewModel();
        }

        public LaterListViewModel ViewModel { get; }

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
            //var flyoutItem = sender as MenuFlyoutItem;
            //var msg = flyoutItem.DataContext as Models.Message.Message;
            //bool result = await _messageService.PinAsync(msg.Id, _session);
            //if (result)
            //{
            //    msg.IsPinned = true;
            //}
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            //var flyoutItem = sender as MenuFlyoutItem;
            //var msg = flyoutItem.DataContext as Models.Message.Message;
            //bool result = await _messageService.UnPinAsync(msg.Id, _session);
            //if (result)
            //{
            //    msg.IsPinned = false;
            //}
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadMessagesAsync();
        }
    }
}
