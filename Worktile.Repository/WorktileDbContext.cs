using Microsoft.EntityFrameworkCore;
using Worktile.Repository.Entities;

namespace Worktile.Repository
{
    public class WorktileDbContext : DbContext
    {
        public DbSet<DownloadTask> DownloadTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Worktile.db");
        }
    }
}
