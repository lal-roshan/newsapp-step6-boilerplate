using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Exceptions;
using UserService.Models;
using UserService.Services;
namespace UserService.Controllers
{
    /*
     * As in this assignment, we are working with creating RESTful web service, hence annotate
     * the class with [ApiController] annotation and define the controller level route as per 
     * REST Api standard.
     * 
     * Authorize the controller by using attribute to the Controller
     */
    public class UserController : ControllerBase
    {
        /*
        UserService should  be injected through constructor injection. 
        Please note that we should not create service
        object using the new keyword
        */
        string userId = string.Empty;
        public UserController(IUserService userService)
        {
            
        }

        /* Implement HttpVerbs and its Functionality asynchronously*/

        /*
         * Define a handler method which will get us the user by a userId.
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news found successfully.
         * 2. Handle Custom Exception when expected userId not found
         * 3. Use HttpGet to get the user by userId
         */

        /*
        * Define a handler method which will create a specific UserProfile by reading the
        * Serialized object from request body and save the user details in a User table
        * in the database.
        * 
        * Please note that AddUser method should add a userdetails and also handle the exception.
        * Dispaly the status messages basis on different situations: 
        * 1. 201(CREATED) - If the userProfile details created successfully. 
        * 2. 409(CONFLICT) - If the userId conflicts with any existing userId
        * 
        * use HTTP POST method to Add user Details.
        */


        /*
       * Define a handler method which will update a specific user by reading the
       * Serialized object from request body and save the updated user details in a
       * user table in database handle exception as well.
       * This handler method should return any one of the status messages basis on different situations: 
       * 1. 200(OK) - If the user updated successfully. 
       * 2. 404(NOT FOUND) - If the user with specified userId is not found. 
       * 
       * use HTTP PUT method.
       */
    }
}
