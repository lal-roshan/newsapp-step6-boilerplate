using AuthenticationService.Models;
using System.Linq;

namespace AuthenticationService.Repository
{
    /// <summary>
    /// Class performing operations on database
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        /// <summary>
        /// readonly property for database context
        /// </summary>
        readonly AuthDbContext dbContext;

        /// <summary>
        /// Constructor for injecting the database context property
        /// </summary>
        /// <param name="dbContext"></param>
        public AuthRepository(AuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Method for creating a new user
        /// </summary>
        /// <param name="user">Details of the user to be created</param>
        /// <returns>True if creation successful else false</returns>
        public bool CreateUser(User user)
        {
            dbContext.Users.Add(user);
            return dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// Method for checking whether a user id is already registered
        /// </summary>
        /// <param name="userId">The id to be checked</param>
        /// <returns>True if user already exists else false</returns>
        public bool IsUserExists(string userId)
        {
            return dbContext.Users.Any(u => string.Equals(userId, u.UserId));
        }

        /// <summary>
        /// Method to check whether credentials matches with any user
        /// </summary>
        /// <param name="user">The credentials to be checked</param>
        /// <returns>True if credentials matches else false</returns>
        public bool LoginUser(User user)
        {
            return dbContext.Users.Any(u => string.Equals(u.UserId, user.UserId)
            && string.Equals(u.Password, user.Password));
        }
    }
}
