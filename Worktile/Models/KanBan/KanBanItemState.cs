namespace Worktile.Models.KanBan
{
    public class KanBanItemState
    {
        public string Foreground { get; set; }
        public string Background => Foreground.Insert(1, "1A");
        public string Glyph { get; set; }
        public string Name { get; set; }
    }
}
