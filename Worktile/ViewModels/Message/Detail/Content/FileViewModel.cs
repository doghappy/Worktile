using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Worktile.ApiModels.Upload;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    class FileViewModel : ViewModel
    {
        public FileViewModel(ISession session)
        {
            _session = session;
        }

        private ISession _session;

        public event PropertyChangedEventHandler PropertyChanged;

        private int RefType => _session.PageType ==  PageType.Channel ? 1 : 2;

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            if (files.Any())
            {
                var client = new WtHttpClient();
                string url = $"{DataSource.ApiUserMeData.Config.Box.BaseUrl}entities/upload?team_id={DataSource.Team.Id}&ref_id={_session.Id}&ref_type={RefType}";
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
    }
}
