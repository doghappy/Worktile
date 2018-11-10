using System.Collections.Generic;
using Windows.Storage;
using Worktile.Models;

namespace Worktile.Services
{
    public static class CommonData
    {
        public static string SubDomain
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(SubDomain)].ToString();
            set => ApplicationData.Current.LocalSettings.Values[nameof(SubDomain)] = value;
        }

        public static string TeamId { get; set; }
        public static string TeamName { get; set; }
        public static string TeamLogo { get; set; }

        public static ApiModel.ApiUserMe.Config ApiUserMeConfig { get; set; }
        //public static ApiModel.ApiUserSignIn.Data Me { get; set; }
        public static ApiModel.ApiTeam.Data Team { get; set; }

        public static readonly List<WtApp> Apps = new List<WtApp>
        {
            new WtApp
            {
                Name = "message",
                DisplayName = "消息",
                Glyph = "\ue61e",
                GlyphO = "\ue618",
                Color = "#22d7bb"
            },
            new WtApp
            {
                Name = "task",
                DisplayName = "任务",
                Glyph = "\ue61a",
                GlyphO = "\ue614",
                Color = "#22D7BB"
            },
            new WtApp
            {
                Name = "calendar",
                DisplayName = "日历",
                Glyph = "\ue619",
                GlyphO = "\ue615",
                Color = "#FFA415"
            },
            new WtApp
            {
                Name = "drive",
                DisplayName = "网盘",
                Glyph = "\ue61c",
                GlyphO = "\ue616",
                Color = "#66C060"
            },
            new WtApp
            {
                Name = "report",
                DisplayName = "简报",
                Glyph = "\ue60c",
                GlyphO = "\ue60d",
                Color = "#2DBCFF"
            },
            new WtApp
            {
                Name = "crm",
                DisplayName = "销售",
                Glyph = "\ue605",
                GlyphO = "\ue60f",
                Color = "#18BFA4"
            },
            new WtApp
            {
                Name = "approval",
                DisplayName = "审批",
                Glyph = "\ue602",
                GlyphO = "\ue611",
                Color = "#99D75A"
            },
            new WtApp
            {
                Name = "bulletin",
                DisplayName = "公告",
                Glyph = "\ue60b",
                GlyphO = "\ue60e",
                Color = "#FF5B57"
            },
            new WtApp
            {
                Name = "leave",
                DisplayName = "考勤",
                Glyph = "\ue610",
                GlyphO = "\ue608",
                Color = "#FFD234"
            },
            new WtApp
            {
                Name = "portal",
                DisplayName = "门户",
                Glyph = "\ue606",
                GlyphO = "\ue610",
                Color = "#C472EE"
            },
            new WtApp
            {
                Name = "appraisal",
                DisplayName = "考核",
                Glyph = "\ue609",
                GlyphO = "\ue601",
                Color = "#7076FA"
            },
            new WtApp
            {
                Name = "okr",
                DisplayName = "目标",
                Glyph = "\ue613",
                GlyphO = "\ue60a",
                Color = "#4E8AF9"
            }
        };
    }
}
