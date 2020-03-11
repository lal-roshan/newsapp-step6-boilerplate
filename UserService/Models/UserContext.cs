using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace UserService.Models
{
    public class UserContext
    {
        //declare variables to connect to MongoDB database
        public UserContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
           
        }
        //Define a MongoCollection to represent the Users collection of MongoDB based on UserProfile type
       
    }
}
