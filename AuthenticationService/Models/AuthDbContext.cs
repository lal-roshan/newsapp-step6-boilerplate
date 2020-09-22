using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Models
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
            //make sure that database is auto generated using EF Core Code first
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
        }

        //Define a Dbset for User in the database
        public DbSet<User> Users { get; set; }
    }
}
