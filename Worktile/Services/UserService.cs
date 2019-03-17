using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Common.Communication;

namespace Worktile.Services
{
    class UserService
    {
        public async Task<Data> GetMeAsync()
        {
            var me = await WtHttpClient.GetAsync<ApiUserMe>("api/user/me");
            return me.Data;
        }
    }
}
