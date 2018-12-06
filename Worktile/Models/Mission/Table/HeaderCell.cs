using Worktile.Enums;

namespace Worktile.Models.Mission.Table
{
    class HeaderCell
    {
        public string Text { get; set; }
        public double Width { get; set; }
        public string Key { get; set; }
        public string RowKey { get; set; }
        public string Lookup { get; set; }
        public WtTaskPropertyType Type { get; set; }
    }
}
