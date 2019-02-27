using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Worktile.ApiModels.Upload;
using Worktile.Common;
using Worktile.Common.WtRequestClient;

namespace Worktile.ViewModels.Message
{
    class FileViewModel : ViewModel
    {
        public FileViewModel(Views.Message.Session session)
        {
            _session = session;
        }

        private Views.Message.Session _session;

        public new event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            if (files.Any())
            {
                var client = new WtHttpClient();
                string url = $"{DataSource.ApiUserMeData.Config.Box.BaseUrl}entities/upload?team_id={DataSource.Team.Id}&ref_id={_session.Id}&ref_type={_session.RefType}";
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
