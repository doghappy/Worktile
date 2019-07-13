using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worktile.Repository.Entities
{
    [Table("DownloadTasks")]
    public class DownloadTask
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Size { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DownloadTaskStatus Status { get; set; }
    }

    public enum DownloadTaskStatus
    {
        Downloading,
        Completed
    }
}
