using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common.Communication;
using Worktile.Enums.Message;
using Worktile.Models.Message;
using System;
using Worktile.Models.Message.Session;

namespace Worktile.Services
{
    class MessageService
    {
        protected virtual string GetMessageUrl(ISession session, int refType, int filterType)
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerable<Message> ReadMessages(JToken jToken)
        {
            throw new NotImplementedException();
        }

        public bool? HasMore { get; protected set; }
        protected virtual string IdType { get; }

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

        public async Task<IEnumerable<Message>> LoadMessagesAsync(ISession session, int filterType)
        {
            int refType = session.PageType == PageType.Channel ? 1 : 2;
            string url = GetMessageUrl(session, refType, filterType);
            var data = await WtHttpClient.GetJTokenAsync(url);
            return ReadMessages(data);
        }

        public async Task<bool> PinAsync(string messageId, string sessionId)
        {
            string url = "/api/pinneds";
            var req = new
            {
                type = 1,
                message_id = messageId,
                worktile = sessionId
            };
            string json = JsonConvert.SerializeObject(req);
            json = json.Replace("worktile", IdType);
            var response = await WtHttpClient.PostAsync<ApiResponse>(url, json);
            if (response.Code == 200)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UnPinAsync(string messageId, string sessionId)
        {
            string url = $"/api/messages/{messageId}/unpinned?{IdType}={sessionId}";
            var response = await WtHttpClient.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ClearUnReadAsync(string sessionId)
        {
            string url = $"/api/messages/unread/clear?ref_id={sessionId}";
            var data = await WtHttpClient.PutAsync<ApiResponse>(url);
            return data.Code == 200;
        }
    }
}
