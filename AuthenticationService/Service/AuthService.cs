using AuthenticationService.Exceptions;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using System;

namespace AuthenticationService.Service
{
    /// <summary>
    /// Class for handling invalid operation scenarios
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// readonly property for repository
        /// </summary>
        readonly IAuthRepository authRepository;

        /// <summary>
        /// Constructor for injecting repository property
        /// </summary>
        /// <param name="authRepository"></param>
        public AuthService(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        /// <summary>
        /// Method for checking whether credentials matches with any user
        /// </summary>
        /// <param name="user">The credentails to be checked</param>
        /// <returns>True if match is found else exception is thrown</returns>
        public bool LoginUser(User user)
        {
            if (authRepository.LoginUser(user))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid user id or password");
            }
        }

        /// <summary>
        /// Method for registering new user
        /// </summary>
        /// <param name="user">Details of new user</param>
        /// <returns>True if registered successfully else exception is thrown</returns>
        public bool RegisterUser(User user)
        {
            if (!authRepository.IsUserExists(user.UserId))
            {
                return authRepository.CreateUser(user);
            }
            else
            {
                throw new UserAlreadyExistsException($"This userId {user.UserId} already in use");
            }
        }
    }
}
