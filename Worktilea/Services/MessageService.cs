﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common.Communication;
using Worktile.Enums.Message;
using Worktile.Models.Message;
using System;
using Worktile.Models.Message.Session;
using Worktile.ApiModels.Message.ApiMessages;
using Worktile.Common;
using Worktile.ApiModels.ApiPinnedMessages;
using Worktile.Enums;

namespace Worktile.Services
{
    class MessageService
    {
        private string _latestId;

        protected virtual string GetMessageUrl(ISession session, int filterType)
        {
            int refType = GetRefType(session.PageType);
            return $"/api/messages?ref_id={session.Id}&ref_type={refType}&latest_id={_latestId}&size=20";
        }

        protected virtual IEnumerable<Message> ReadMessages(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiMessages>();
            foreach (var item in apiData.Data.Messages)
            {
                item.ContentFormat();
                MessageHelper.SetAvatar(item);
                yield return item;
            }
            _latestId = apiData.Data.LatestId;
            HasMore = apiData.Data.More;
        }

        public bool? HasMore { get; protected set; }

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

        public static int GetRefType(PageType type)
        {
            return type == PageType.Channel ? 1 : 2;
        }

        public async Task<IEnumerable<Message>> LoadMessagesAsync(ISession session, int filterType)
        {
            string url = GetMessageUrl(session, filterType);
            var data = await WtHttpClient.GetJTokenAsync(url);
            return ReadMessages(data);
        }

        public async Task<bool> PinAsync(string messageId, ISession session)
        {
            string url = "/api/pinneds";
            var req = new
            {
                type = 1,
                message_id = messageId,
                worktile = session.Id
            };
            string json = JsonConvert.SerializeObject(req);
            string idType = session.GetType() == typeof(ChannelSession) ? "channel_id" : "session_id";
            json = json.Replace("worktile", idType);
            var response = await WtHttpClient.PostAsync<ApiResponse>(url, json);
            if (response.Code == 200)
            {
                return true;
            }
            return false;
        }

        private string GetIdType(ISession session)
        {
            return session.GetType() == typeof(ChannelSession) ? "channel_id" : "session_id";
        }

        public async Task<bool> UnPinAsync(string messageId, ISession session)
        {
            string url = $"/api/messages/{messageId}/unpinned?{GetIdType(session)}={session.Id}";
            var response = await WtHttpClient.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ClearUnReadAsync(string sessionId)
        {
            string url = $"api/messages/unread/clear?ref_id={sessionId}";
            var data = await WtHttpClient.PutAsync<ApiResponse>(url);
            return data.Code == 200;
        }

        public virtual async Task<ApiPinnedMessages> GetPinnedMessagesAsync(ISession session, string anchor)
        {
            string url = $"api/pinneds?session_id={session.Id}&anchor={anchor}&size=10";
            return await WtHttpClient.GetAsync<ApiPinnedMessages>(url);
        }

        public async Task<bool> ExitChannelAsync(string sessionId)
        {
            string url = $"api/channels/{sessionId}/leave";
            var res = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            return res.Code == 200 && res.Data;
        }

        public async Task<List<ChannelSession>> GetAllChannelsAsync()
        {
            string url = "api/channels?type=all&filter=all&status=ok";
            var data = await WtHttpClient.GetAsync<ApiDataResponse<List<ChannelSession>>>(url);
            return data.Data;
        }

        public async Task<bool> ActiveChannelAsync(string sessionId)
        {
            string url = $"api/channels/{sessionId}/active";
            var data = await WtHttpClient.PutAsync<ApiResponse>(url);
            return data.Code == 200;
        }

        public async Task<bool> JoinChannelAsync(string sessionId)
        {
            string url = $"api/channels/{sessionId}/join";
            var data = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            return data.Code == 200 && data.Data;
        }

        public async Task<MemberSession> CreateSessionAsync(string uid)
        {
            var data = await WtHttpClient.PostAsync<ApiDataResponse<MemberSession>>("/api/session", new { uid });
            if (data.Code == 200)
            {
                return data.Data;
            }
            return null;
        }

        public async Task<ApiDataResponse<ChannelSession>> CreateChannelAsync(string uids, string name, string color, string desc, WtVisibility visibility)
        {
            var req = new
            {
                name,
                color,
                desc,
                default_uids = uids,
                visibility
            };
            return await WtHttpClient.PostAsync<ApiDataResponse<ChannelSession>>("api/channel", req);
        }

        public async Task<bool> AddMemberToChannelAsync(string sessionId, string uid)
        {
            string url = $"api/channels/{sessionId}/invite";
            var req = new { uid };
            var res = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url, req);
            return res.Code == 200 && res.Data;
        }
    }
}
