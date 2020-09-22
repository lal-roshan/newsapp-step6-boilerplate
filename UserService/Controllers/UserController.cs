using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Services;
namespace UserService.Controllers
{
    /// <summary>
    /// Api controller class for User entitiy
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// readonly property for service class
        /// </summary>
        readonly IUserService userService;

        /// <summary>
        /// Parametrised constructor for injecting the service property
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Http Get method for getting the details of a user
        /// </summary>
        /// <response code="200">If user details was fetched successfully</response>
        /// <response code="401">If unauthorized to perform</response>
        /// <response code="404">If user not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "userId"));
                if (userId != null)
                {
                    return Ok(await userService.GetUser(userId.Value));
                }
                return Unauthorized();
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /// <summary>
        /// Http post for adding new user
        /// </summary>
        /// <param name="user">The details of the user to be added</param>
        /// <response code="201">If user was added successfully</response>
        /// <response code="401">If unauthorized to perform</response>
        /// <response code="409">If user was already present</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(UserProfile user)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "userId"));
                if (userId != null && string.Equals(userId.Value, user.UserId))
                {
                    bool added = await userService.AddUser(user);
                    return Created("api/user", added);
                }
                return Unauthorized($"Your credentials doesn't match User Profile");
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /// <summary>
        /// Http Put method for updating the details of a user
        /// </summary>
        /// <param name="user">The user object with new details</param>
        /// <response code="200">If user details was updated successfully</response>
        /// <response code="401">If unauthorized to perform</response>
        /// <response code="404">If user not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(UserProfile user)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "userId"));
                if (userId != null && string.Equals(userId.Value, user.UserId))
                {
                    return Ok(await userService.UpdateUser(userId.Value, user));
                }
                return Unauthorized($"You are not allowed to update {user.UserId} Profile");
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }
    }
}
