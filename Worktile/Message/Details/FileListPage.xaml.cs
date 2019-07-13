using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.Networking.BackgroundTransfer;
using Worktile.Common;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Worktile.Main;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Threading;
using System.Linq;
using Worktile.Tool;
using Worktile.Repository;
using Worktile.Repository.Entities;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Worktile.Message.Details
{
    public sealed partial class FileListPage : Page
    {
        public FileListPage()
        {
            InitializeComponent();
            ViewModel = new FileListViewModel();
        }

        public FileListViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
        }

        private void AppBarToggleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var btn = sender as AppBarToggleButton;
            ViewModel.SelectionMode = btn.IsChecked.Value
                ? ListViewSelectionMode.Multiple
                : ListViewSelectionMode.Single;
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            var allDownloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (var item in FilesListView.SelectedItems)
            {
                var entity = item as Entity;
                string url = UtilityTool.GetS3FileUrl(entity.Id);
                if (!allDownloads.Any(d => d.RequestedUri.ToString() == url))
                {
                    var file = await DownloadsFolder.CreateFileAsync(entity.Addition.Title, CreationCollisionOption.GenerateUniqueName);
                    var downloader = new BackgroundDownloader();
                    //downloader.SuccessToastNotification = new ToastNotification();
                    //downloader.FailureToastNotification = new ToastNotification();
                    SendToast(entity.Addition.Title, url);
                    await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        var download = downloader.CreateDownload(new Uri(url), file);
                        download.RangesDownloaded += Download_RangesDownloaded;
                        await download.StartAsync();
                    }));
                }
            }

            string downloading = UtilityTool.GetStringFromResources("Downloading");
            string content = UtilityTool.GetStringFromResources("DownloadFileDialogContent");
            var dialog = new ContentDialog
            {
                Title = downloading,
                Content = content,
                PrimaryButtonText = "OK"
            };
            await dialog.ShowAsync();
        }

        private void SendToast(string fileName, string url)
        {
            string downloading = UtilityTool.GetStringFromResources("Downloading");
            ToastContent content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = $"{downloading} {fileName}..."
                            },
                            new AdaptiveProgressBar()
                            {
                                Title = fileName,
                                Value = new BindableProgressBarValue("progressValue"),
                                Status = new BindableString("progressStatus")
                            }
                        }
                    }
                }
            };
            var toast = new ToastNotification(content.GetXml())
            {
                Tag = UtilityTool.GetIdFromS3FileUrl(url),
                Group = "Download"
            };
            toast.Data = new NotificationData();
            toast.Data.Values["progressValue"] = "0";
            toast.Data.Values["progressStatus"] = $"{downloading}...";
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void Download_RangesDownloaded(DownloadOperation sender, BackgroundTransferRangesDownloadedEventArgs args)
        {
            string tag = UtilityTool.GetIdFromS3FileUrl(sender.RequestedUri);
            var entity = ViewModel.Entities.Single(e => e.Id == tag);
            string downloading = UtilityTool.GetStringFromResources("Downloading");
            string downloadCompleted = UtilityTool.GetStringFromResources("DownloadCompleted");
            var data = new NotificationData();
            double progressValue = sender.Progress.BytesReceived / (ulong)entity.Addition.Size;
            data.Values["progressValue"] = progressValue.ToString();
            if (progressValue < 1)
            {
                data.Values["progressStatus"] = $"{downloading}...";
            }
            else
            {
                data.Values["progressStatus"] = $"{downloadCompleted}";
            }
            ToastNotificationManager.CreateToastNotifier().Update(data, tag, "Download");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            ViewModel.DownloadIsEnabled = listView.SelectedItems.Count > 0;
        }
    }
}
