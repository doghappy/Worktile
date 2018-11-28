using System.Collections.Generic;

namespace Worktile.Models.Mission.AnalyticInsight
{
    public class TopNavItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public List<TopNavItem> SubItems { get; set; }
    }
}
