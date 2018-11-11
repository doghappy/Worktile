using System.Collections.Generic;

namespace Worktile.Models.KanBan
{
    public class KanBanGroup
    {
        public KanBanGroup()
        {
            Items = new List<KanBanItem>();
        }

        public string Header { get; set; }
        public int NotStarted { get; set; }
        public int Processing { get; set; }
        public int Completed { get; set; }
        public List<KanBanItem> Items { get; }
    }
}
