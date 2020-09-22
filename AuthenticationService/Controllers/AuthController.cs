﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Exceptions;
using AuthenticationService.Models;
using AuthenticationService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
namespace AuthenticationService.Controllers
{
    /* Annotate the class with [ApiController] annotation and define the controller level 
     * route as per REST Api standard.
     */
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        /*
         AuthService should  be injected through constructor injection. Please note that we should not create service
         object using the new keyword
        */
        readonly IAuthService authService;

        readonly ITokenGeneratorService tokenGeneratorService;

        public AuthController(IAuthService authService, ITokenGeneratorService tokenGeneratorService)
        {
            this.authService = authService;
            this.tokenGeneratorService = tokenGeneratorService;
        }

        /*
       * Define a handler method which will create a specific user by reading the
       * Serialized object from request body and save the user details in the
       * database. This handler method should return any one of the status messages
       * basis on different situations:
       * 1. 201(CREATED) - If the user created successfully. 
       * 2. 409(CONFLICT) - If the userId conflicts with any existing user
       * 
       * This handler method should map to the URL "/api/auth/register" using HTTP POST method
       */
        [HttpPost("register")]
        [ActionName("Post")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                bool registered = await authService.RegisterUser(user);
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

        /* Define a handler method which will authenticate a user by reading the Serialized user
        * object from request body containing the username and password. The username and password 
        * should be validated before proceeding ahead with JWT token generation. The user credentials 
        * will be validated against the database entries. 
        * 
        * The error should be return if validation is not successful. If credentials are validated successfully, 
        * then JWT token will be generated. The token should be returned back to the caller along with the API 
        * response. This handler method should return any one of the status messages basis on different
        * situations:
        * 1. 200(OK) - If login is successful
        * 2. 401(UNAUTHORIZED) - If login is not successful
        * 
        * This handler method should map to the URL "/api/auth/login" using HTTP POST method
       */
        [HttpPost("login")]
        [ActionName("Post")]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                if (await authService.LoginUser(user))
                {
                    return Ok(tokenGeneratorService.GenerateToken(user.UserId));
                }
                else
                {
                    return Unauthorized("Invalid user id or password");
                }
            }
            catch (UserNotFoundException ex)
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
