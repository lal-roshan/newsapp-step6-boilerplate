using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace ReminderService.Models
{
    /// <summary>
    /// Class representing reminder document
    /// </summary>
    public class Reminder
    {
        [BsonId]
        public string UserId { get; set; }

        public string Email { get; set; }

        public List<ReminderSchedule> NewsReminders { get; set; }

    }
}
