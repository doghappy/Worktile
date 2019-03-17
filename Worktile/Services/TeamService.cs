using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common.Communication;
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
    }
}
