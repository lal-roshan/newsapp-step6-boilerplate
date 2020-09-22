using MongoDB.Driver;
using ReminderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ReminderService.Repository
{
    /// <summary>
    /// Class for fascilitating CRUD operations on reminder entity
    /// </summary>
    public class ReminderRepository : IReminderRepository
    {
        /// <summary>
        /// readonly property for database context
        /// </summary>
        readonly ReminderContext reminderContext;

        /// <summary>
        /// Parametrised constructor for injecting database context
        /// </summary>
        /// <param name="reminderContext"></param>
        public ReminderRepository(ReminderContext reminderContext)
        {
            this.reminderContext = reminderContext;
        }

        /// <summary>
        /// Method for creating reminder
        /// </summary>
        /// <param name="userId">The id of the user against whom the reminder is to be added</param>
        /// <param name="email">The email of the user</param>
        /// <param name="schedule">The properties of the reminder</param>
        /// <returns></returns>
        public async Task CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            var builder = Builders<Reminder>.Filter;
            var userFilter = builder.Eq(r => r.UserId, userId) & builder.Eq(r => r.Email, email);

            /// Finding whether user exists or not
            var result = await reminderContext.Reminders.FindAsync(userFilter);
            var userNews = await result?.FirstOrDefaultAsync();

            ///If user doesnt exists create a new document
            if (userNews == null)
            {
                var newUser = new Reminder
                {
                    UserId = userId,
                    Email = email,
                    NewsReminders = new List<ReminderSchedule>
                    {
                        new ReminderSchedule
                        {
                            NewsId = schedule.NewsId,
                            Schedule = schedule.Schedule
                        }
                    }
                };
                await reminderContext.Reminders.InsertOneAsync(newUser);
            }
            ///If user exists add to users reminders
            else
            {
                var update = Builders<Reminder>.Update.Push(r => r.NewsReminders, schedule);
                await reminderContext.Reminders.UpdateOneAsync(userFilter, update, new UpdateOptions { IsUpsert = true });
            }
        }

        /// <summary>
        /// Method for deleting a reminder
        /// </summary>
        /// <param name="userId">The id of the user whose reminder is to be deleted</param>
        /// <param name="newsId">The id of the news whose reminder is to be deleted</param>
        /// <returns>True if deletion successful</returns>
        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var builder = Builders<Reminder>.Filter;

            ///Pulls out the matching reminder from the rest of the list
            var pull = Builders<Reminder>.Update.PullFilter(r => r.NewsReminders, n => n.NewsId == newsId);

            ///Filter for finding the document containing the userId and newsId provided
            var filter = builder.Eq(r => r.UserId, userId)
                & builder.ElemMatch(r => r.NewsReminders, n => n.NewsId == newsId);

            ///Updates the matching document with list of reminders from which we have pulled out the matching reminder
            var result = await reminderContext.Reminders.UpdateOneAsync(filter, pull);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        /// <summary>
        /// Method for getting all reminders of a user
        /// </summary>
        /// <param name="userId">The id of the user whose reminders are to be fetched</param>
        /// <returns>The list of reminders added by the user</returns>
        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            var result = await reminderContext.Reminders.FindAsync(r => string.Equals(r.UserId, userId));
            var reminders = await result.FirstOrDefaultAsync();
            return reminders?.NewsReminders;
        }

        /// <summary>
        /// Method for checking whether a reminder exists against a news for a user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="newsId">The id of the news</param>
        /// <returns>True if reminder is present for the news of the user</returns>
        public async Task<bool> IsReminderExists(string userId, int newsId)
        {
            var builder = Builders<Reminder>.Filter;

            ///Filter for finding matching document
            var filter = builder.Eq(r => r.UserId, userId) &
                builder.ElemMatch(r => r.NewsReminders, n => n.NewsId == newsId);
            var result = await reminderContext.Reminders.FindAsync(filter);
            if (result != null && await result.FirstOrDefaultAsync() != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method for updating a reminder
        /// </summary>
        /// <param name="userId">The id of the user whose reminder is to be updated</param>
        /// <param name="reminder">New properties of the reminder to be updated</param>
        /// <returns>True if updation successful</returns>
        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            var builder = Builders<Reminder>.Filter;

            ///Filter for finding matching document
            var filter = builder.Eq(r => r.UserId, userId)
                & builder.ElemMatch(r => r.NewsReminders, n => n.NewsId == reminder.NewsId);

            ///Update for setting the reminders value
            var update = Builders<Reminder>.Update.Set("NewsReminders.$.Schedule", reminder.Schedule);

            ///Update the reminder of the matching document with new value
            var result = await reminderContext.Reminders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
