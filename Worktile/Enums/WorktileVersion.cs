using System.ComponentModel;

namespace Worktile.Enums
{
    public enum WorktileVersion
    {
        [Description("试用版")]
        Trail = -1,

        [Description("免费版")]
        Free = 1,

        [Description("专业版")]
        Professional,

        [Description("企业版")]
        Enterprise,

        [Description("旗舰版")]
        Ultimate
    }
}
