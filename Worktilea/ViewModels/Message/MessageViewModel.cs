﻿using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common.Communication;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message
{
    abstract class MessageViewModel : ViewModel
    {
        public async Task StarSessionAsync(ISession session)
        {
            string sessionType = session.GetType() == typeof(ChannelSession) ? "channels" : "sessions";
            string url = $"/api/{sessionType}/{session.Id}/star";
            var data = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                session.Starred = true;
            }
        }

        public async Task UnStarSessionAsync(ISession session)
        {
            string sessionType = session.GetType() == typeof(ChannelSession) ? "channels" : "sessions";
            string url = $"/api/{sessionType}/{session.Id}/unstar";
            var data = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                session.Starred = false;
            }
        }
    }
}
