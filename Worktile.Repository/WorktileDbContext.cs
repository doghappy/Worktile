using Microsoft.EntityFrameworkCore;
using System;
using Worktile.Repository.Entities;

namespace Worktile.Repository
{
    public class WorktileDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Worktile.db");
        }
    }
}
