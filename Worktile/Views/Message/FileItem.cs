using System;

namespace Worktile.Views.Message
{
    class FileItem
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string FileName { get; set; }
        public string Size { get; set; }
        public TethysAvatar Avatar { get; set; }
        public DateTime DateTime { get; set; }
    }
}
