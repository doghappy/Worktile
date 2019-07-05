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

namespace Worktile.Message.Details
{
    public sealed partial class MessageListPage : Page
    {
        public MessageListPage()
        {
            InitializeComponent();
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
    }
}
