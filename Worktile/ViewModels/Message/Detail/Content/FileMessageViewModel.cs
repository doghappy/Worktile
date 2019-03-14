using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Worktile.ApiModels.Upload;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Enums.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    abstract class FileMessageViewModel<S> : ViewModel where S : ISession
    {
        protected FileMessageViewModel(S session)
        {
            Session = session;
        }

        protected S Session { get; }
        protected MainViewModel MainViewModel { get; }
        protected int RefType => Session.PageType == PageType.Channel ? 1 : 2;
        protected int ToType => Session.GetType() == typeof(ChannelSession) ? 1 : 2;

        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            if (files.Any())
            {
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
                        await WtHttpClient.PostAsync<ApiEntitiesUpload>(url, content);
                    }
                }
            }
        }

        public async Task DownloadFileAsync(StorageFile storageFile, string fileId)
        {
            string url = WtFileHelper.GetS3FileUrl(fileId);
            var buffer = await WtHttpClient.GetByteBufferAsync(url);
            await FileIO.WriteBufferAsync(storageFile, buffer);
        }

        public async Task<bool> SendMessageAsync(string msg)
        {
            var data = new
            {
                fromType = 1,
                from = DataSource.ApiUserMeData.Me.Uid,
                to = Session.Id,
                toType = ToType,
                messageType = 1,
                client = 1,
                markdown = 1,
                content = msg
            };
            return await WtSocket.SendMessageAsync(SocketMessageType.Message, data);
        }
    }
}
