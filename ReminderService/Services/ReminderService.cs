using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ReminderService.Services
{
    public class ReminderService
    {
        /*Inherit the respective interface and implement the methods in 
         the class i.e ReminderService by inheriting IReminderService
         */

        /* ReminderRepository should  be injected through constructor injection. 
         * Please note that we should not create ReminderRepository object using the new keyword
         */
       
        public ReminderService(IReminderRepository reminderRepository)
        {
            
        }
        /* Implement all the methods of respective interface asynchronously*/

        /* Implement GetReminders method which should be used to get all reminders by userId.*/

        /* Implement CreateReminder method which should be used to create a new reminder using userId, email
           and reminder Object*/

        /* Implement DeleteReminder method which should be used to delete a reminder by userId and newsId*/

        /* Implement a UpdateReminder method which should be used to update an existing reminder by using
         userId and reminder details*/
    }
}
