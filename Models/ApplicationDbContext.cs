using DailyHelper.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DailyHelper.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ToDoTask> Tasks { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<IdentityUser> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                .HasKey(n => n.Id);
            modelBuilder.Entity<Note>()
                .Property(n => n.Title)
                .IsRequired();

            modelBuilder.Entity<ToDoTask>()
                .HasKey(n => n.Id);
            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.Title)
                .IsRequired();
            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.DueDate)
                .IsRequired();
        }
    }
}