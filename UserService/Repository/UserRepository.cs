using MongoDB.Driver;
using System.Threading.Tasks;
using UserService.Models;
namespace UserService.Repository
{
    /// <summary>
    /// Repository class for fascilitating CRUD operations on user document
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// readonly property for database context
        /// </summary>
        readonly UserContext userContext;

        /// <summary>
        /// Parametrised constructor for injecting data context
        /// </summary>
        /// <param name="userContext"></param>
        public UserRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }

        /// <summary>
        /// The method for adding user
        /// </summary>
        /// <param name="user">The user details that is to be added</param>
        /// <returns>True if inserted successfully else false</returns>
        public async Task<bool> AddUser(UserProfile user)
        {
            await userContext.Users.InsertOneAsync(user);
            var result = await userContext.Users.FindAsync(u => u.UserId == user.UserId);
            return await result.AnyAsync();
        }

        /// <summary>
        /// Method for getting details of a user
        /// </summary>
        /// <param name="userId">The id of the user whose details are to be fetched</param>
        /// <returns>The user profile of the requested user</returns>
        public async Task<UserProfile> GetUser(string userId)
        {
            var result = await userContext.Users.FindAsync(u => u.UserId == userId);
            return await result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Method to update the details of a user
        /// </summary>
        /// <param name="user">The details of the user that is to be updated with</param>
        /// <returns>True if updation was successful else false</returns>
        public async Task<bool> UpdateUser(UserProfile user)
        {
            var filter = Builders<UserProfile>.Filter.Where(u => u.UserId == user.UserId);
            var update = Builders<UserProfile>.Update
                .Set(u => u.FirstName, user.FirstName)
                .Set(u => u.LastName, user.LastName)
                .Set(u => u.Contact, user.Contact)
                .Set(u => u.Email, user.Email);
            var result = await userContext.Users.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
