using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.Message.ApiMessageFiles;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;
using Worktile.Services;
using Worktile.ViewModels.Message.Detail.Content;
using Worktile.Views.Message.Dialog;

namespace Worktile.Views.Message.Detail.Content
{
    public sealed partial class FilePage : Page, INotifyPropertyChanged
    {
        public FilePage()
        {
            InitializeComponent();
            _fileService = new FileService();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly FileService _fileService;
        private ISession _session;
        private int _page = 1;
        private int _refType;

        //public IncrementalCollection<FileItem> Files { get; private set; }
        private IncrementalCollection<FileItem> _files;
        public IncrementalCollection<FileItem> Files
        {
            get => _files;
            set
            {
                if (_files != value)
                {
                    _files = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Files)));
                }
            }
        }


        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var masterPage = this.GetParent<MasterPage>();
            _session = masterPage.SelectedSession;
            _refType = _session.PageType == PageType.Channel ? 1 : 2;

            Files = new IncrementalCollection<FileItem>(LoadFilesAsync);
        }

        private async Task<IEnumerable<FileItem>> LoadFilesAsync()
        {
            IsActive = true;
            var list = new List<FileItem>();
            var data = await _fileService.GetFilesAsync(_page, _refType, _session.Id);
            Files.HasMoreItems = Files.Count + data.Data.Entities.Count < data.Data.Total;
            _page++;
            foreach (var item in data.Data.Entities)
            {
                list.Add(new FileItem
                {
                    Id = item.Id,
                    Icon = WtFileHelper.GetFileIcon(item.Addition.Ext),
                    FileName = item.Addition.Title,
                    Size = GetFriendlySize(item.Addition.Size),
                    Avatar = new TethysAvatar
                    {
                        DisplayName = item.CreatedBy.DisplayName,
                        Background = AvatarHelper.GetColorBrush(item.CreatedBy.DisplayName),
                        Source = AvatarHelper.GetAvatarBitmap(item.CreatedBy.Avatar, AvatarSize.X40, FromType.User)
                    },
                    DateTime = item.CreatedAt,
                    IsEnableDelete = item.CreatedBy.Uid == DataSource.ApiUserMeData.Me.Uid,
                    IsEnableDownload = !string.IsNullOrEmpty(item.Addition.Path)
                });
            }

            IsActive = false;
            return list;
        }

        private string GetFriendlySize(int size)
        {
            if (size <= 0)
            {
                return "0B";
            }
            else
            {
                var units = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
                int index = 0;
                double cursor = size * 1.0;
                while (cursor >= 1024)
                {
                    index++;
                    cursor /= 1024;
                }
                return cursor.ToString("0.00").Replace(".00", string.Empty) + " " + units[index];
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
            await _fileService.UploadFileAsync(files, _session.Id, _refType);
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            IsActive = true;
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
                await _fileService.DownloadFileAsync(storageFile, file.Id);
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
            IsActive = false;
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
                IsActive = true;
                var control = sender as MenuFlyoutItem;
                var file = control.DataContext as FileItem;
                bool res = await _fileService.DeleteFileAsync(file.Id);
                if (res)
                {
                    Files.Remove(file);
                }
                IsActive = false;
            }
        }

        private async void FileShare_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FileShareDialg();
            await dialog.ShowAsync();
        }
    }
}
