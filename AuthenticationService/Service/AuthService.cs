using AuthenticationService.Exceptions;
using AuthenticationService.Models;
using AuthenticationService.Repository;
namespace AuthenticationService.Service
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e AuthService by inheriting IAuthService class 
    //which is used to implement all methods in the classs.
    public class AuthService
    {
        //define a private variable to represent repository

        //Use constructor Injection to inject all required dependencies.
        public AuthService(IAuthRepository authRepository)
        {

        }
        /* Implement all the methods of respective interface asynchronously*/

        //Implement the method  'RegisterUser' which is used to register a new user and 
        // handle the Custom Exception for UserAlreadyExistsException


        //Implement the method 'LoginUser' which is used to login existing user and also handle the Custom Exception 

    }
}
