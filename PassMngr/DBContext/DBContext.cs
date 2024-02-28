using Microsoft.EntityFrameworkCore;
using PassMngr.Models;

namespace PassMngr.DBContext
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(t => t.password_list)
                .WithOne()
                .HasForeignKey(i => i.user_id);

            // Other configurations as needed
        }
    }
    
}
