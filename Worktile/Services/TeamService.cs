using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Department;
using Worktile.Models.Team;

namespace Worktile.Services
{
    class TeamService
    {
        public async Task<Team> GetTeamAsync()
        {
            var data = await WtHttpClient.GetAsync<ApiDataResponse<Team>>("api/team");
            return data.Data;
        }

        public async Task<Worktile.ApiModels.ApiTeamChats.Data> GetChatAsync()
        {
            var data = await WtHttpClient.GetAsync<Worktile.ApiModels.ApiTeamChats.ApiTeamChats>("/api/team/chats");
            return data.Data;
        }

        public async Task<List<DepartmentNode>> GetDepartmentsTreeAsync()
        {
            string url = $"/api/departments/tree?async=false";
            var data = await WtHttpClient.GetAsync<ApiDataResponse<List<DepartmentNode>>>(url);
            return data.Data;
        }
    }
}
