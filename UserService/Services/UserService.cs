using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
namespace UserService.Services
{
    /// <summary>
    /// Service facilitating operations on user documents
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// readonly property for repository
        /// </summary>
        readonly IUserRepository userRepository;

        /// <summary>
        /// Parametrised constructor for injecting repository
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Method to add user
        /// </summary>
        /// <param name="user">The user data that is to be added</param>
        /// <returns>Returns true if added successfully else false</returns>
        public async Task<bool> AddUser(UserProfile user)
        {
            var presentUser = await userRepository.GetUser(user.UserId);
            if (presentUser == null)
            {
                return await userRepository.AddUser(user);
            }
            else
            {
                throw new UserAlreadyExistsException($"{user.UserId} is already in use");
            }
        }

        /// <summary>
        /// Method to get details of a particular user
        /// </summary>
        /// <param name="userId">The id of the user whose details are to be fetched</param>
        /// <returns>Returns the user profile of the requested user</returns>
        public async Task<UserProfile> GetUser(string userId)
        {
            var presentUser = await userRepository.GetUser(userId);
            if (presentUser != null)
            {
                return presentUser;
            }
            else
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
        }

        /// <summary>
        /// Method for updating details of a particular user
        /// </summary>
        /// <param name="userId">The id of the user to be updated</param>
        /// <param name="user">The details that is to be applied</param>
        /// <returns>Returns true if update was successful</returns>
        public async Task<bool> UpdateUser(string userId, UserProfile user)
        {
            var presentUser = await userRepository.GetUser(userId);
            if (presentUser != null)
            {
                return await userRepository.UpdateUser(user);
            }
            else
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
        }
    }
}
