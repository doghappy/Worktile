using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Worktile.ApiModels.Message.ApiMessageFiles;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;
using Worktile.ApiModels;

namespace Worktile.ViewModels.Message.Detail.Content
{
    class FileViewModel : FileMessageViewModel<ISession>, INotifyPropertyChanged
    {
        public FileViewModel(ISession session) : base(session)
        {
            Files = new IncrementalCollection<FileItem>(LoadFilesAsync);
        }

        int _page = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public IncrementalCollection<FileItem> Files { get; }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async Task<IEnumerable<FileItem>> LoadFilesAsync()
        {
            IsActive = true;
            var list = new List<FileItem>();
            string url = $"/api/entities?page={_page}&size=20&ref_type={RefType}&ref_id={Session.Id}";
            var data = await WtHttpClient.GetAsync<ApiMessageFiles>(url);
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

        public async Task<bool> DeleteFileAsync(string fileId)
        {
            IsActive = true;
            string url = $"/api/entities/{fileId}";
            var res = await WtHttpClient.DeleteAsync<ApiDataResponse<bool>>(url);
            IsActive = false;
            return res.Code == 200 && res.Data;
        }
    }
}
