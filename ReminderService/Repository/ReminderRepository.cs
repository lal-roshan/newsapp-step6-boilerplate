using MongoDB.Driver;
using ReminderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e ReminderRepository by inheriting IReminderRepository class 
    //which is used to implement all Data access operations
    public class ReminderRepository
    {
        //define a private variable to represent Reminder Database Context
        public ReminderRepository(ReminderContext reminderContext)
        {
           
        }
        //Implement the methods of interface Asynchronously.

        // Implement CreateReminder method which should be used to save a new reminder.  

        // Implement DeleteReminder method which should be used to delete an existing reminder.

        // Implement GetReminders method which should be used to get a reminder by userId.

        // Implement IsReminderExists method which should be used to check an existing reminder by newsId

        // Implement UpdateReminder method which should be used to update an existing reminder using  userId and 
        // reminder Schedule
    }
}
