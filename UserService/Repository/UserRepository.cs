using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
namespace UserService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserRepository by inheriting IUserRepository class 
    //which is used to implement all methods in the classs
    public class UserRepository
    {
        //define a private variable to represent UserContext 
       
        public UserRepository(UserContext userContext)
        {
            
        }
        //Implement the methods of interface Asynchronously.

        // Implement AddUser method which should be used to add  a new user Profile. 

        // Implement GetUser method which should be used to get a user by userId.

        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
