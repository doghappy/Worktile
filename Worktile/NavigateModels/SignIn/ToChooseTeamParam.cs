using System.Collections.Generic;
using Worktile.Models.Team;

namespace Worktile.NavigateModels.SignIn
{
    class ToChooseTeamParam
    {
        public string PassToken { get; set; }
        public List<Team> Teams { get; set; }
    }
}
