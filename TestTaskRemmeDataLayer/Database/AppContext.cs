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
                .HasMany(c => c.Todos)
                .WithOne(e => e.User);

            modelBuilder.Entity<Todo>().Property(a => a.IsDone).HasDefaultValue(false);
            //modelBuilder.Entity<User>().Property(a => a.Todos).IsConcurrencyToken();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
    }
}