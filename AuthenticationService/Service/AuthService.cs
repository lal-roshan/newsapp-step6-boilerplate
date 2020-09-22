using AuthenticationService.Exceptions;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using System.Threading.Tasks;

namespace AuthenticationService.Service
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e AuthService by inheriting IAuthService class 
    //which is used to implement all methods in the classs.
    public class AuthService : IAuthService
    {
        //define a private variable to represent repository
        readonly IAuthRepository authRepository;

        //Use constructor Injection to inject all required dependencies.
        public AuthService(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        public async Task<bool> LoginUser(User user)
        {
            if (await authRepository.LoginUser(user))
            {
                return true;
            }
            else
            {
                throw new UserNotFoundException("Invalid user id or password");
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            if(await authRepository.CreateUser(user))
            {
                return true;
            }
            else
            {
                throw new UserAlreadyExistsException($"This userId {user.UserId} already in use");
            }
        }
        /* Implement all the methods of respective interface asynchronously*/

        //Implement the method  'RegisterUser' which is used to register a new user and 
        // handle the Custom Exception for UserAlreadyExistsException


        //Implement the method 'LoginUser' which is used to login existing user and also handle the Custom Exception 

    }
}
