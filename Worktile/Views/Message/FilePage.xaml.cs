using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.Message.ApiMessageFiles;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;

namespace Worktile.Views.Message
{
    public sealed partial class FilePage : Page, INotifyPropertyChanged
    {
        public FilePage()
        {
            InitializeComponent();
            Files = new IncrementalCollection<FileItem>(LoadFilesAsync);
        }

        Session _session;

        public event PropertyChangedEventHandler PropertyChanged;

        IncrementalCollection<FileItem> Files { get; }

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _session = e.Parameter as Session;
        }

        int _page = 1;

        private async Task<IEnumerable<FileItem>> LoadFilesAsync()
        {
            IsActive = true;
            var list = new List<FileItem>();
            string url = $"/api/entities?page={_page}&size=20&ref_type={_session.RefType}&ref_id={_session.Id}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMessageFiles>(url);
            Files.HasMoreItems = Files.Count + data.Data.Entities.Count < data.Data.Total;
            foreach (var item in data.Data.Entities)
            {
                list.Add(new FileItem
                {
                    Id = item.Id,
                    Icon = GetIcon(item.Addition.Ext),
                    FileName = item.Addition.Title,
                    Size = GetFriendlySize(item.Addition.Size),
                    Avatar = new TethysAvatar
                    {
                        DisplayName = item.CreatedBy.DisplayName,
                        Background = AvatarHelper.GetColorBrush(item.CreatedBy.DisplayName),
                        Source = AvatarHelper.GetAvatarBitmap(item.CreatedBy.Avatar, AvatarSize.X40, FromType.User)
                    },
                    DateTime = item.CreatedAt
                });
            }

            IsActive = false;
            return list;
        }

        private string GetIcon(string ext)
        {
            string name = "default";
            switch (ext)
            {
                case "doc":
                case "docx":
                    name = "doc";
                    break;
                case "ppt":
                case "pptx":
                    name = "ppt";
                    break;
                case "xls":
                case "xlsx":
                    name = "xls";
                    break;
                case "pdf":
                    name = "pdf";
                    break;
                case "txt":
                    name = "txt";
                    break;
                case "apk":
                    name = "apk";
                    break;
                case "bak":
                    name = "bak";
                    break;
                case "cs":
                    name = "cs";
                    break;
                case "csv":
                    name = "csv";
                    break;
                case "exe":
                    name = "exe";
                    break;
                case "fla":
                    name = "fla";
                    break;
                case "html":
                    name = "html";
                    break;
                case "ipa":
                    name = "ipa";
                    break;
                case "java":
                    name = "java";
                    break;
                case "js":
                    name = "js";
                    break;
                case "mp3":
                    name = "mp3";
                    break;
                case "mp4":
                    name = "mp4";
                    break;
                case "php":
                    name = "php";
                    break;
                case "rar":
                    name = "rar";
                    break;
                case "swf":
                    name = "swf";
                    break;
                case "ttf":
                    name = "ttf";
                    break;
                case "vss":
                    name = "vss";
                    break;
                case "xsd":
                    name = "xsd";
                    break;
                case "zip":
                    name = "zip";
                    break;
                case "youdaonote":
                    name = "youdaonote";
                    break;
                case "evernote":
                    name = "evernote";
                    break;
                case "yinxiang":
                    name = "yinxiang";
                    break;
                case "quip":
                    name = "quip";
                    break;
                case "onenote":
                    name = "onenote";
                    break;
                case "onedrive":
                    name = "onedrive";
                    break;
                case "box":
                    name = "box";
                    break;
                case "shimo":
                    name = "shimo";
                    break;
                case "processon":
                    name = "processon";
                    break;
            }
            return $"{DataSource.ApiUserMeData.Config.CdnRoot}image/icons/{name}.png";
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
                int index = -1;
                while (true)
                {
                    index++;
                    if (size / 1024 < 1024)
                    {
                        break;
                    }
                }
                return size % 1024 + units[index];
            }
        }
    }
}
