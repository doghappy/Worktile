using System;

namespace Worktile.Models.Message
{
    public class FileItem
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string FileName { get; set; }
        public string Size { get; set; }
        public TethysAvatar Avatar { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsEnableDelete { get; set; }
        public bool IsEnableDownload { get; set; }
    }
}
