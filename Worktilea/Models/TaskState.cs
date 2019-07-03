namespace Worktile.Models
{
    public class TaskState
    {
        public string Id { get; set; }
        public string Foreground { get; set; }
        public string Background => Foreground.Insert(1, "1a");
        public string BoldGlyph { get; set; }
        public string LightGlyph { get; set; }
        public string Name { get; set; }
    }
}
