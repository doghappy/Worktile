using Microsoft.Toolkit.Uwp.Helpers;
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
using Worktile.Common.Communication;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Entity;
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
            _entityService = new EntityService();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly EntityService _entityService;
        private ISession _session;
        private int _page = 1;
        private int _refType;

        private IncrementalCollection<Entity> _entities;
        public IncrementalCollection<Entity> Entities
        {
            get => _entities;
            set
            {
                if (_entities != value)
                {
                    _entities = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Entities)));
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
            Entities = new IncrementalCollection<Entity>(LoadFilesAsync);
            WtSocket.OnFeedReceived += OnFeedReceived;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            WtSocket.OnFeedReceived -= OnFeedReceived;
        }

        private async Task<IEnumerable<Entity>> LoadFilesAsync()
        {
            IsActive = true;
            var list = new List<Entity>();
            var data = await _entityService.GetFilesAsync(_page, _refType, _session.Id);
            Entities.HasMoreItems = Entities.Count + data.Data.Entities.Count < data.Data.Total;
            _page++;
            foreach (var item in data.Data.Entities)
            {
                item.ForShowEntity();
                list.Add(item);
            }

            IsActive = false;
            return list;
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
            IsActive = true;
            await _entityService.UploadFileAsync(files, _session.Id, _refType);
            IsActive = false;
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            var control = sender as MenuFlyoutItem;
            control.IsEnabled = false;
            var entity = control.DataContext as Entity;
            string ext = Path.GetExtension(entity.Addition.Title);
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
                SuggestedFileName = entity.Addition.Title,
                DefaultFileExtension = ext
            };
            picker.FileTypeChoices.Add(string.Empty, new List<string>() { ext });
            var storageFile = await picker.PickSaveFileAsync();
            if (storageFile != null)
            {
                await _entityService.DownloadFileAsync(storageFile, entity.Id);
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
                                    Text = $"文件“{entity.Addition.Title}”下载完成。"
                                }
                            }
                        }
                    }
                };
                var toastNotif = new ToastNotification(toastContent.GetXml());
                ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
            }
            IsActive = false;
            control.IsEnabled = false;
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
                var entity = control.DataContext as Entity;
                bool res = await _entityService.DeleteFileAsync(entity.Id);
                if (res)
                {
                    Entities.Remove(entity);
                }
                IsActive = false;
            }
        }

        private async void FileShare_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FileShareDialg();
            await dialog.ShowAsync();
        }

        private async void OnFeedReceived(Feed feed)
        {
            if (feed.Type == FeedType.AddFile)
            {
                await Task.Run(async () =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        IsActive = true;
                        var entity = await _entityService.GetFileAsync(feed.EntityId);
                        entity.ForShowEntity();
                        Entities.Insert(0, entity);
                        IsActive = false;
                    });
                });
            }
        }
    }
}
