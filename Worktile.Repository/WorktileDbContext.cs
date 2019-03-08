using Microsoft.EntityFrameworkCore;
using System;
using Worktile.Repository.Entities;

namespace Worktile.Repository
{
    public class WorktileDbContext : DbContext
    {
        public DbSet<SignInfo> SignInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=worktile.db");
        }
    }
}
