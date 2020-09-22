using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Services
{
    /// <summary>
    /// The service class for reminder entity
    /// </summary>
    public class ReminderService : IReminderService
    {
        /// <summary>
        /// readonly property for repository
        /// </summary>
        readonly IReminderRepository reminderRepository;

        /// <summary>
        /// Parametrised constructor for injecting repository
        /// </summary>
        /// <param name="reminderRepository"></param>
        public ReminderService(IReminderRepository reminderRepository)
        {
            this.reminderRepository = reminderRepository;
        }

        /// <summary>
        /// Method for creating reminder
        /// </summary>
        /// <param name="userId">The user id against whom the reminder is to be added</param>
        /// <param name="email">The email of the user</param>
        /// <param name="schedule">The reminder details</param>
        /// <returns>True if creation successful</returns>
        public async Task<bool> CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            if (!await reminderRepository.IsReminderExists(userId, schedule.NewsId))
            {
                await reminderRepository.CreateReminder(userId, email, schedule);
                return true;
            }
            else
            {
                throw new ReminderAlreadyExistsException($"This News already have a reminder");
            }
        }

        /// <summary>
        /// Method for deleting a reminder
        /// </summary>
        /// <param name="userId">The user whose reminder is to be deleted</param>
        /// <param name="newsId">The news id to which the reminder is to be added</param>
        /// <returns>Returns true if deletion was successful</returns>
        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            if (await reminderRepository.IsReminderExists(userId, newsId))
            {
                return await reminderRepository.DeleteReminder(userId, newsId);
            }
            else
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
        }

        /// <summary>
        /// Method for getting all reminders of a user
        /// </summary>
        /// <param name="userId">The id of the user whose reminders are to be fetched</param>
        /// <returns>The list of reminders added by the user</returns>
        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            var reminders = await reminderRepository.GetReminders(userId);
            if (reminders != null && reminders.Any())
            {
                return reminders;
            }
            else
            {
                throw new NoReminderFoundException("No reminders found for this user");
            }
        }

        /// <summary>
        /// Method for updating a reminder
        /// </summary>
        /// <param name="userId">The id of the user whose reminder is to be updated</param>
        /// <param name="reminder">The new details of the reminder to be applied</param>
        /// <returns>True if updation was successful</returns>
        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            var updated = await reminderRepository.UpdateReminder(userId, reminder);
            if (updated)
            {
                return updated;
            }
            else
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
        }
    }
}
