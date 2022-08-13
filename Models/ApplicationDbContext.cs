using DailyHelper.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DailyHelper.Models
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<ToDoTask> Tasks { get; set; }
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; }
        public virtual DbSet<ShopItem> ShopItems { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<IdentityUser> Users { get; set; }

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

        public DbSet<DailyHelper.Entity.ShopItem> ShopItem { get; set; }
    }
}