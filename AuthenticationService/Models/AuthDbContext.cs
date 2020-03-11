using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Models
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
              //make sure that database is auto generated using EF Core Code first
        }

        //Define a Dbset for User in the database
    }
}
