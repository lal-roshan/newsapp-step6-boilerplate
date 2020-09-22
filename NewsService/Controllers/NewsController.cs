using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsService.Exceptions;
using NewsService.Models;
using NewsService.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace NewsService.Controllers
{
    /// <summary>
    /// Api controller class for News entity
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class NewsController : ControllerBase
    {
        /// <summary>
        /// readonly property for service class
        /// </summary>
        readonly INewsService newsService;

        /// <summary>
        /// Parametrised constructor for injecting the service property
        /// </summary>
        /// <param name="newsService"></param>
        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        /// <summary>
        /// Http Get method for getting the news of a user with provided user Id
        /// </summary>
        /// <param name="userId">The id of the user whose news are to be fetched</param>
        /// <response code="200">If news were fetched successfully</response>
        /// <response code="404">If no news were found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c, "userId"));
                if (userId != null)
                {
                    return Ok(await newsService.FindAllNewsByUserId(userId.Value));
                }
                return BadRequest();
            }
            catch (NoNewsFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /// <summary>
        /// Http post for adding new reminder
        /// </summary>
        /// <param name="userId">The id of the user to which news is to be added</param>
        /// <param name="news">The details of the news to be added</param>
        /// <response code="201">If reminder was added successfully</response>
        /// <response code="409">If reminder was already present</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(News news)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c, "userId"));
                if (userId != null)
                {
                    int newsId = await newsService.CreateNews(userId.Value, news);
                    return Created("api/news", newsId);
                }
                return BadRequest();
            }
            catch (NewsAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /// <summary>
        /// Http Delete for deleting a news
        /// </summary>
        /// <param name="userId">The id of the user whose news is to be deleted</param>
        /// <param name="newsId">The id of the news to be deleted</param>
        /// <response code="200">If news was deleted successfully</response>
        /// <response code="404">If news was not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpDelete("{userId}/{newsId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int newsId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c, "userId"));
                if (userId != null)
                {
                    return Ok(await newsService.DeleteNews(userId.Value, newsId));
                }
                return BadRequest();
            }
            catch (NoNewsFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /// <summary>
        /// Http Delete for deleting a reminder
        /// </summary>
        /// <param name="userId">The id of the user whose reminder is to be deleted</param>
        /// <param name="newsId">The id of the news having the reminder to be deleted</param>
        /// <response code="200">If reminder was deleted successfully</response>
        /// <response code="404">If reminder was not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [ActionName("Delete")]
        [HttpDelete("{userId}/{newsId:int}/reminder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReminder(int newsId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c, "userId"));
                if (userId != null)
                {
                    return Ok(await newsService.DeleteReminder(userId.Value, newsId));
                }
                return BadRequest();
            }
            catch (NoReminderFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /// <summary>
        /// Http Put method for updating the details of a reminder
        /// </summary>
        /// <param name="userId">The id of the user whose reminder is to be updated</param>
        /// <param name="reminder">The reminder object with new details</param>
        /// <response code="200">If reminder details was updated successfully</response>
        /// <response code="404">If reminder not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPut("{userId}/{newsId:int}/reminder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int newsId, Reminder reminder)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c, "userId"));
                if (userId != null)
                {
                    return Ok(await newsService.AddOrUpdateReminder(userId.Value, newsId, reminder));
                }
                return BadRequest();
            }
            catch (NoNewsFoundException ex)
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
