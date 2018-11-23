using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Worktile.Models.Kanban
{
    public class KanbanItem
    {
        public string Id { get; set; }
        public Avatar Avatar { get; set; }
        public TaskState State { get; set; }
        public string Title { get; set; }
        public SolidColorBrush Priority { get; set; }
        public TaskType TaskType { get; set; }
        public List<KanbanItemProperty> Properties { get; set; }
    }
}
