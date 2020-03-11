using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;
namespace AuthenticationService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e AuthRepository by inheriting IAuthRepository class 
    //which is used to implement all methods in the classs.
    public class AuthRepository
    {
        //Define a private variable to represent AuthDbContext
        public AuthRepository(AuthDbContext dbContext)
        {
         
        }

        /* Implement all the methods of respective interface asynchronously*/

        //Implement the method  'CreateUser' which is used to create a new user.

        //Implement the method  'IsUserExists' which is used to check userId exist or not.

        //Implement the method 'LoginUser' which is used to login for the existing user.
    }
}
