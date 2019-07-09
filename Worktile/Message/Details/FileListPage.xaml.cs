﻿using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.Networking.BackgroundTransfer;
using Worktile.Common;
using System.IO;

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
            foreach (var item in FilesListView.SelectedItems)
            {
                var entity = item as Entity;
                Uri uri = new Uri(UtilityTool.GetS3FileUrl(entity.Id));
                var file = await DownloadsFolder.CreateFileAsync(entity.Addition.Title, CreationCollisionOption.GenerateUniqueName);
                var downloader = new BackgroundDownloader();
                var download =  downloader.CreateDownload(uri, file);
                await download.StartAsync();

            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            ViewModel.DownloadIsEnabled = listView.SelectedItems.Count > 0;
        }
    }
}
