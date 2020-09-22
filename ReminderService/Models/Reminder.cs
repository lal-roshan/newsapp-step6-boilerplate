using System;
namespace ReminderService.Models
{
    /// <summary>
    /// Class representing the properties of a reminder 
    /// </summary>
    public class ReminderSchedule
    {
        public int NewsId { get; set; }

        public DateTime Schedule { get; set; }
    }
}
