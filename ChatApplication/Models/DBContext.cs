using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
                   : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<AvailableUsers> AvailableUsers { get; set; }
        public virtual DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity mappings and relationships here
            modelBuilder.Entity<UserRoles>()
                .HasKey(sc => new { sc.userId, sc.roleId });

            // For simplicity, let's assume there are no additional configurations needed
            base.OnModelCreating(modelBuilder);
        }
    }
}
