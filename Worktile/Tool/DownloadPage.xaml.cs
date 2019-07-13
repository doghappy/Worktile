using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Tool.Models;

namespace Worktile.Tool
{
    public sealed partial class DownloadPage : Page
    {
        public DownloadPage()
        {
            InitializeComponent();
            Downloads = new ObservableCollection<WtDownloadOperation>();
        }

        public ObservableCollection<WtDownloadOperation> Downloads { get; }

        public ObservableCollection<DownloadOperation> Tasks { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (var item in downloads)
            {
                var download = new WtDownloadOperation
                {
                    Name = item.ResultFile.Name,
                    Extension = item.ResultFile.FileType,
                    DownloadOperation = item
                };
                Downloads.Add(download);
                await download.DownloadOperation
                    .AttachAsync()
                    .AsTask(CancellationToken.None, new Progress<DownloadOperation>(ProgressChanged));
            }
        }

        private void ProgressChanged(DownloadOperation download)
        {
            var item = Downloads.Single(d => d.DownloadOperation == download);
            item.BytesReceived = download.Progress.BytesReceived;
            item.TotalBytesToReceive = download.Progress.TotalBytesToReceive;
            if (item.TotalBytesToReceive != 0)
            {
                item.ProgressRate = item.BytesReceived / item.TotalBytesToReceive;
            }
        }
    }
}
