using System.Collections.Generic;

namespace Worktile.Models.Kanban
{
    public class KanbanGroup
    {
        public KanbanGroup()
        {
            Items = new List<KanbanItem>();
        }

        public string Header { get; set; }
        public int NotStarted { get; set; }
        public int Processing { get; set; }
        public int Completed { get; set; }
        public List<KanbanItem> Items { get; }
    }
}
