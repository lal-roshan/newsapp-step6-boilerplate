using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Models
{
    /// <summary>
    /// Class fascilitating communication with database
    /// </summary>
    public class AuthDbContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public AuthDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Method invoked on model creation
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
        }

        /// <summary>
        /// Representing the users table in database
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
