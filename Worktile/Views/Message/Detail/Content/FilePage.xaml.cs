using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message.Detail.Content;
using Worktile.Views.Message.Dialog;

namespace Worktile.Views.Message.Detail.Content
{
    public sealed partial class FilePage : Page, INotifyPropertyChanged
    {
        public FilePage()
        {
            InitializeComponent();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var session = e.Parameter as ISession;
            ViewModel = new FileViewModel(session);
        }

        private FileViewModel _viewModel;
        private FileViewModel ViewModel
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

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Downloads
            };
            picker.FileTypeFilter.Add("*");
            var files = await picker.PickMultipleFilesAsync();
            await ViewModel.UploadFileAsync(files);
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsActive = true;
            var control = sender as MenuFlyoutItem;
            var file = control.DataContext as FileItem;
            string ext = Path.GetExtension(file.FileName);
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
                SuggestedFileName = file.FileName,
                DefaultFileExtension = ext
            };
            picker.FileTypeChoices.Add(string.Empty, new List<string>() { ext });
            var storageFile = await picker.PickSaveFileAsync();
            if (storageFile != null)
            {
                await ViewModel.DownloadFileAsync(storageFile, file.Id);
                var toastContent = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = $"文件“{file.FileName}”下载完成。"
                                }
                            }
                        }
                    }
                };
                var toastNotif = new ToastNotification(toastContent.GetXml());
                ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
            }
            ViewModel.IsActive = false;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "确认删除文件",
                Content = "删除文件后，你将看不到该文件的任何信息，同时关于该文件的消息也将删除。",
                PrimaryButtonText = "确定删除",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var control = sender as MenuFlyoutItem;
                var file = control.DataContext as FileItem;
                bool res = await ViewModel.DeleteFileAsync(file.Id);
                if (res)
                {
                    ViewModel.Files.Remove(file);
                }
            }
        }

        private async void FileShare_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FileShareDialg
            {
                SendMessage = ViewModel.SendMessageAsync
            };
            //dialog.PrimaryButtonClick += (s, args) =>
            //{
            //    if (dialog.SelectedItem != null)
            //    {

            //    }
            //};
            await dialog.ShowAsync();
        }
    }
}
