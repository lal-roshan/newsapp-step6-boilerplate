using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        /// Http Get method for getting the details of a user with provided user Id
        /// </summary>
        /// <param name="userId">The id of the user whose details is to be fetched</param>
        /// <response code="200">If user details was fetched successfully</response>
        /// <response code="404">If user not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                return Ok(await userService.GetUser(userId));
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
        /// <response code="409">If user was already present</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(UserProfile user)
        {
            try
            {
                bool added = await userService.AddUser(user);
                return Created("api/user", added);
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
        /// <param name="userId">The id of the user whose details are to be updated</param>
        /// <param name="user">The user object with new details</param>
        /// <response code="200">If user details was updated successfully</response>
        /// <response code="404">If user not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(string userId, UserProfile user)
        {
            try
            {
                return Ok(await userService.UpdateUser(userId, user));
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
