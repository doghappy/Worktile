using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Worktile.ApiModels.Message.ApiMessageFiles;
using Worktile.ApiModels.Upload;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;
using Worktile.ApiModels;

namespace Worktile.ViewModels.Message.Detail.Content
{
    class FileViewModel : MessageBaseViewModel<ISession>
    {
        public FileViewModel(ISession session) : base(session)
        {
            Files = new IncrementalCollection<FileItem>(LoadFilesAsync);
        }

        int _page = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public IncrementalCollection<FileItem> Files { get; }

        protected override string IdType { get; }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async Task<IEnumerable<FileItem>> LoadFilesAsync()
        {
            IsActive = true;
            var list = new List<FileItem>();
            string url = $"/api/entities?page={_page}&size=20&ref_type={RefType}&ref_id={Session.Id}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMessageFiles>(url);
            Files.HasMoreItems = Files.Count + data.Data.Entities.Count < data.Data.Total;
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

        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            if (files.Any())
            {
                var client = new WtHttpClient();
                string url = $"{DataSource.ApiUserMeData.Config.Box.BaseUrl}entities/upload?team_id={DataSource.Team.Id}&ref_id={Session.Id}&ref_type={RefType}";
                foreach (var file in files)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        string fileName = file.DisplayName + file.FileType;
                        var content = new MultipartFormDataContent
                        {
                            { new StringContent(fileName), "name" },
                            { new StreamContent(stream), "file", fileName }
                        };
                        await client.PostAsync<ApiEntitiesUpload>(url, content);
                    }
                }
            }
        }

        public async Task DownloadFileAsync(StorageFile storageFile, string fileId)
        {
            string url = WtFileHelper.GetS3FileUrl(fileId);
            var client = new WtHttpClient();
            var buffer = await client.GetByteArrayAsync(url);
            await FileIO.WriteBytesAsync(storageFile, buffer);
        }

        public async Task<bool> DeleteFileAsync(string fileId)
        {
            IsActive = true;
            string url = $"/api/entities/{fileId}";
            var client = new WtHttpClient();
            var res = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            IsActive = false;
            return res.Code == 200 && res.Data;
        }
    }
}
