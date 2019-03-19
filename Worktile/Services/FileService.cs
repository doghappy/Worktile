using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Worktile.ApiModels.Upload;
using Worktile.Common;
using Worktile.Common.Communication;
using Windows.Web.Http;

namespace Worktile.Services
{
    class FileService
    {
        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files, string sessionId, int refType)
        {
            if (files.Any())
            {
                string url = $"{DataSource.ApiUserMeData.Config.Box.BaseUrl}entities/upload?team_id={DataSource.Team.Id}&ref_id={sessionId}&ref_type={refType}";
                foreach (var file in files)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
                    using (var stream = await file.OpenReadAsync())
                    {
                        string fileName = file.DisplayName + file.FileType;
                        var content = new HttpMultipartFormDataContent
                        {
                            { new HttpStringContent(fileName), "name" },
                            { new HttpStreamContent(stream), "file", fileName }
                        };
                        await WtHttpClient.PostAsync<ApiEntitiesUpload>(url, content);
                    }
                }
            }
        }
    }
}
