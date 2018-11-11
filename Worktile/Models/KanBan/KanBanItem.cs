namespace Worktile.Models.KanBan
{
    public class KanBanItem
    {
        public KanBanItemAvatar Avatar { get; set; }
        public KanBanItemState State { get; set; }
        public string Title { get; set; }
        public string Identifier { get; set; }
        public KanBanItemTaskType TaskType { get; set; }
    }
}
