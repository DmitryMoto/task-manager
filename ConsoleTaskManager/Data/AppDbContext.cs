using ConsoleTaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTaskManager.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<AppTask> Tasks { get; set; }
        public DbSet<People> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=ConsoleManagerApp.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppTask>().HasKey(t => t.Id);
            modelBuilder.Entity<AppTask>().Property(t => t.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<AppTask>().HasOne(t => t.Author).WithMany(a => a.AuthoredTasks).HasForeignKey(t => t.AuthorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AppTask>().HasOne(t => t.Assignee).WithMany(a => a.AssignedTasks).HasForeignKey(t => t.AssigneeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
