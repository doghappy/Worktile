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
using Worktile.Common;
using Worktile.Models.Message.Session;
using Worktile.Services;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class ChannelSettingPage : Page
    {
        public ChannelSettingPage()
        {
            InitializeComponent();
            _channelMessageService = new ChannelMessageService();
        }

        readonly ChannelMessageService _channelMessageService;
        private ISession _session;
        private MasterPage _masterPage;
        private ChannelDetailPage _detailPage;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _detailPage = this.GetParent<ChannelDetailPage>();
            _masterPage = _detailPage.GetParent<MasterPage>();
            _session = _detailPage.Session;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditChannelPage));
        }

        private async void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "归档群组",
                Content = "当群组不再使用时，你可以归档它，归档后群组中的历史消息仍然可以被搜索，确定要归档群组吗？",
                PrimaryButtonText = "确定归档",
                SecondaryButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await _channelMessageService.ArchiveAsync(_session.Id);
                _masterPage.Sessions.Remove(_session);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "删除群组",
                Content = "当群组不再使用并且不想保留消息历史时，你可以删除它，群组中的历史消息都将被删除，确定要删除群组吗？",
                PrimaryButtonText = "确定删除",
                SecondaryButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await _channelMessageService.DeleteAsync(_session.Id);
            }
        }
    }
}
