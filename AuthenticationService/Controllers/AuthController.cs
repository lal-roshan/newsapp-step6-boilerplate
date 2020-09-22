using AuthenticationService.Exceptions;
using AuthenticationService.Models;
using AuthenticationService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
namespace AuthenticationService.Controllers
{
    /// <summary>
    /// Api controller class for authentication
    /// </summary>
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        /// <summary>
        /// readonly property for authorization service class
        /// </summary>
        readonly IAuthService authService;

        /// <summary>
        /// readonly property for token generation service class
        /// </summary>
        readonly ITokenGeneratorService tokenGeneratorService;

        /// <summary>
        /// Parametrised constructor for injecting the authorization service property
        /// and initialising token generation property
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
            tokenGeneratorService = new TokenGeneratorService();
        }

        /// <summary>
        /// Http post method for registering new users
        /// </summary>
        /// <param name="user">The user data to be registered</param>
        /// <response code="201">If user gets registered successfully</response>
        /// <response code="409">If user already exists</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPost("register")]
        [ActionName("Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register(User user)
        {
            try
            {
                bool registered = authService.RegisterUser(user);
                return Created("api/auth/register", registered);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again later!!");
            }
        }

        /// <summary>
        /// Http post method for user log in
        /// </summary>
        /// <param name="user">The credentials of user for login</param>
        /// <response code="200">If login is successfull</response>
        /// <response code="401">If login fails</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPost("login")]
        [ActionName("Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login(User user)
        {
            try
            {
                if (authService.LoginUser(user))
                {
                    return Ok(tokenGeneratorService.GenerateToken(user.UserId));
                }
                else
                {
                    return Unauthorized("Invalid user id or password");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again later!!");
            }
        }

    }
}
