using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace NewsService.Models
{
    public class NewsContext
    {
        //declare variables to connect to MongoDB database
        public NewsContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
        }

        //Define a MongoCollection to represent the News collection of MongoDB based on UserNews type


    }
}
