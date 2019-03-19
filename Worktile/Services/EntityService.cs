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
using Worktile.ApiModels.Message.ApiMessageFiles;
using Worktile.ApiModels;
using Worktile.Models.Entity;

namespace Worktile.Services
{
    class EntityService
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

        public async Task<ApiMessageFiles> GetFilesAsync(int page, int refType, string sessionId)
        {
            string url = $"api/entities?page={page}&size=20&ref_type={refType}&ref_id={sessionId}";
            return await WtHttpClient.GetAsync<ApiMessageFiles>(url);
        }

        public async Task<Entity> GetFileAsync(string id)
        {
            string url = $"api/entities/{id}";
            var data = await WtHttpClient.GetAsync<ApiDataResponse<Entity>>(url);
            return data.Data;
        }

        public async Task DownloadFileAsync(StorageFile storageFile, string fileId)
        {
            string url = WtFileHelper.GetS3FileUrl(fileId);
            var buffer = await WtHttpClient.GetByteBufferAsync(url);
            await FileIO.WriteBufferAsync(storageFile, buffer);
        }

        public async Task<bool> DeleteFileAsync(string fileId)
        {
            string url = $"api/entities/{fileId}";
            var res = await WtHttpClient.DeleteAsync<ApiDataResponse<bool>>(url);
            return res.Code == 200 && res.Data;
        }
    }
}
