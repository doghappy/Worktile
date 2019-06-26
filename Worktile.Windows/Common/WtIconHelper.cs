using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worktile.Windows.Common
{
    public static class WtIconHelper
    {
        public static string GetAppIcon(string app)
        {
            switch (app)
            {
                case "message": return "\ue618";
                case "task": return "\ue614";
                case "calendar": return "\ue619";
                case "drive": return "\ue616";
                case "report": return "\ue60c";
                case "crm": return "\ue605";
                case "approval": return "\ue602";
                case "bulletin": return "\ue60b";
                case "leave": return "\ue607";
                case "portal": return "\ue606";
                case "appraisal": return "\ue609";
                case "okr": return "\ue613";
                case "mission": return "\ue70d";
                default: return null;
            }
        }
    }
}
