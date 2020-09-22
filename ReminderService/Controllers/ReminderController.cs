using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Controllers
{
    /// <summary>
    /// Api controller class for Reminder entity
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class ReminderController : ControllerBase
    {
        /// <summary>
        /// readonly property for service class
        /// </summary>
        readonly IReminderService reminderService;

        /// <summary>
        /// Parametrised constructor for injecting the service property
        /// </summary>
        /// <param name="userService"></param>
        public ReminderController(IReminderService reminderService)
        {
            this.reminderService = reminderService;
        }

        /// <summary>
        /// Http Get method for getting the reminders of a user
        /// </summary>
        /// <response code="200">If reminders were fetched successfully</response>
        /// <response code="401">If unauthorized to perform</response>
        /// <response code="404">If no reminders was found</response>
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
                    return Ok(await reminderService.GetReminders(userId.Value));
                }
                return Unauthorized();
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
        /// Http post for adding new reminder
        /// </summary>
        /// <param name="reminder">The details of the reminder to be added</param>
        /// <response code="201">If reminder was added successfully</response>
        /// <response code="401">If unauthorized to perform</response>
        /// <response code="409">If reminder was already present</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Reminder reminder)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "userId"));
                if (userId != null && string.Equals(userId.Value, reminder.UserId))
                {
                    bool created = await reminderService.CreateReminder(reminder.UserId,
                        reminder.Email, reminder.NewsReminders.FirstOrDefault());
                    return Created("api/reminder", created);
                }
                return Unauthorized();
            }
            catch (ReminderAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Some error occurred, please try again!!");
            }
        }

        /// <summary>
        /// Http Delete for deleting a reminder
        /// </summary>
        /// <param name="newsId">The id of the news having the reminder to be deleted</param>
        /// <response code="200">If reminder was deleted successfully</response>
        /// <response code="401">If unauthorized to perform</response>
        /// <response code="404">If reminder was not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int newsId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "userId"));
                if (userId != null)
                {
                    return Ok(await reminderService.DeleteReminder(userId.Value, newsId));
                }
                return Unauthorized();
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
        /// <param name="reminder">The reminder object with new details</param>
        /// <response code="200">If reminder details was updated successfully</response>
        /// <response code="401">If unauthorized to perform</response>
        /// <response code="404">If reminder not found</response>
        /// <response code="500">If some error occurs</response>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(ReminderSchedule reminder)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => string.Equals(c.Type, "userId"));
                if (userId != null)
                {
                    return Ok(await reminderService.UpdateReminder(userId.Value, reminder));
                }
                return Unauthorized();
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
    }
}
