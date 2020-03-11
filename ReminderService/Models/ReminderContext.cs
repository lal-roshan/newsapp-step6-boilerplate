using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace ReminderService.Models
{
    public class ReminderContext
    {
        //declare variables to connect to MongoDB database
        public ReminderContext(IConfiguration configuration)
        {

            //Initialize MongoClient and Database using connection string and database name from configuration
        }
        //Define a MongoCollection to represent the Reminders collection of MongoDB based on Reminder type
    }
}
