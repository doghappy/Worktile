using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Worktile.Models.Mission.Table
{
    class Cell
    {
        public string TaskId { get; set; }
        public string RenderParam { get; set; }
        public string Text { get; set; }
        public string Glyph { get; set; }
        public Avatar Avatar { get; set; }
        public List<Avatar> Avatars { get; set; }
        public TaskState TaskState { get; set; }
        public TaskType TaskType { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public SolidColorBrush Background { get; set; }
        public double Width { get; set; }
        public List<TagCell> Tags { get; set; }
        public int Actual { get; set; }
        public int Expected { get; set; }
    }
}
