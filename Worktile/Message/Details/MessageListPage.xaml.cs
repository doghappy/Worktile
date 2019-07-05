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
using WtMessage = Worktile.Message.Models;

namespace Worktile.Message.Details
{
    public sealed partial class MessageListPage : Page
    {
        public MessageListPage()
        {
            InitializeComponent();
            ViewModel = new MessageListViewModel();
        }

        public MessageListViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //await SendMessageAsync();
        }

        private async void AttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            //var picker = new FileOpenPicker
            //{
            //    ViewMode = PickerViewMode.Thumbnail,
            //    SuggestedStartLocation = PickerLocationId.Downloads
            //};
            //picker.FileTypeFilter.Add("*");
            //var files = await picker.PickMultipleFilesAsync();
            //await _entityService.UploadFileAsync(files, _session.Id, RefType);
        }

        private async void MsgTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //if (e.Key == VirtualKey.Enter)
            //{
            //    if (Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down))
            //    {
            //        await SendMessageAsync();
            //    }
            //    else
            //    {
            //        int index = MsgTextBox.SelectionStart;
            //        MsgTextBox.Text = MsgTextBox.Text.Insert(index, Environment.NewLine);
            //        MsgTextBox.SelectionStart = index + 1;
            //    }
            //}
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
