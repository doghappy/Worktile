using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common.Communication;
using Worktile.Models.Message.Session;

namespace Worktile.Services
{
    class MessageService
    {
        public async Task<bool> StarSessionAsync(ISession session)
        {
            string sessionType = session.GetType() == typeof(ChannelSession) ? "channels" : "sessions";
            string url = $"/api/{sessionType}/{session.Id}/star";
            var data = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            return data.Code == 200 && data.Data;
        }

        public async Task<bool> UnStarSessionAsync(ISession session)
        {
            string sessionType = session.GetType() == typeof(ChannelSession) ? "channels" : "sessions";
            string url = $"/api/{sessionType}/{session.Id}/unstar";
            var data = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            return data.Code == 200 && data.Data;
        }

        public async Task<bool> DeleteSessionAsync(string sessionId)
        {
            string url = $"/api/sessions/{sessionId}";
            var data = await WtHttpClient.DeleteAsync<ApiDataResponse<bool>>(url);
            return data.Code == 200 && data.Data;
        }
    }
}
