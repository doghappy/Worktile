using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Common.Communication;
using Worktile.Models.Member;

namespace Worktile.Services
{
    class UserService
    {
        public async Task<Data> GetMeAsync()
        {
            var me = await WtHttpClient.GetAsync<ApiUserMe>("api/user/me");
            return me.Data;
        }

        public async Task<Member> GetMemberInfoAsync(string uid)
        {
            string url = $"api/users/{uid}/basic";
            var data = await WtHttpClient.GetAsync<ApiDataResponse<Member>>(url);
            return data.Data;
        }

        public async Task<string[]> GetFollowsAsync()
        {
            string url = "api/follows";
            var data = await WtHttpClient.GetAsync<ApiDataResponse<string[]>>(url);
            return data.Data;
        }
    }
}
