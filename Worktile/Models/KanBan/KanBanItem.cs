namespace Worktile.Models.KanBan
{
    public class KanBanItem
    {
        public string Id { get; set; }
        public Avatar Avatar { get; set; }
        public TaskState State { get; set; }
        public string Title { get; set; }
        public string Identifier { get; set; }
        public TaskType TaskType { get; set; }
    }
}
