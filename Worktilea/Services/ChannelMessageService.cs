using System.Threading.Tasks;
using Worktile.Common.Communication;
using Worktile.Models.Message.Session;
using Worktile.ApiModels.ApiPinnedMessages;
using Worktile.ApiModels;

namespace Worktile.Services
{
    class ChannelMessageService : MessageService
    {
        public override async Task<ApiPinnedMessages> GetPinnedMessagesAsync(ISession session, string anchor)
        {
            string url = $"api/pinneds?channel_id={session.Id}&anchor={anchor}&size=10";
            return await WtHttpClient.GetAsync<ApiPinnedMessages>(url);
        }

        public async Task<bool> UpdateChannelAsync(string sessionId, string name, string desc, string color)
        {
            string url = $"api/channels/{sessionId}";
            var req = new
            {
                color,
                desc,
                name
            };
            var res = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url, req);
            return res.Code == 200 && res.Data;
        }

        public async Task<bool> ArchiveAsync(string sessionId)
        {
            string url = $"api/channels/{sessionId}/archive";
            var res = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            return res.Code == 200 && res.Data;
        }

        public async Task<bool> DeleteAsync(string sessionId)
        {
            string url = $"api/channels/{sessionId}";
            var res = await WtHttpClient.DeleteAsync<ApiDataResponse<bool>>(url);
            return res.Code == 200 && res.Data;
        }

        public async Task<bool> DeleteMemberFromChannelAsync(string sessionId, string uid)
        {
            string url = $"api/channels/{sessionId}/members/{uid}";
            var res = await WtHttpClient.DeleteAsync<ApiDataResponse<bool>>(url);
            return res.Code == 200 && res.Data;
        }
    }
}
