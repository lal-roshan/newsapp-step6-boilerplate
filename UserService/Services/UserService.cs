using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
namespace UserService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserService by inheriting IUserService
    public class UserService
    {
        /*
         * UserRepository should  be injected through constructor injection. 
         * Please note that we should not create USerRepository object using the new keyword
         */
        public UserService(IUserRepository userRepository)
        {
           
        }
        //Implement the methods of interface Asynchronously.
        // Implement AddUser method which should be used to add  a new user Profile.  
        // Implement GetUser method which should be used to get a user by userId.
        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
