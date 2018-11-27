using Windows.UI.Xaml.Media;

namespace Worktile.Models.Kanban
{
    public class KanbanItemProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public SolidColorBrush Background { get; set; }
    }
}
