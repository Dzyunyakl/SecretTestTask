using Microsoft.EntityFrameworkCore;
using TestTaskRemmeDataLayer.Models;

namespace TestTaskRemmeDataLayer.Database
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(c => c.Tasks)
                .WithOne(e => e.User);

            modelBuilder.Entity<Task>().Property(a => a.IsDone).HasDefaultValue(false);
            //modelBuilder.Entity<User>().Property(a => a.Tasks).IsConcurrencyToken();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}