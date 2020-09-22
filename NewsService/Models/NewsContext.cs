using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace NewsService.Models
{
    /// <summary>
    /// Class fascilitating communication with database
    /// </summary>
    public class NewsContext
    {
        /// <summary>
        /// Represents the mongo client
        /// </summary>
        readonly MongoClient mongoClient;

        /// <summary>
        /// Represents the database that we operate upon
        /// </summary>
        readonly IMongoDatabase mongoDb;

        /// <summary>
        /// Constructor receiving the configuration file details and initialising mongo entities
        /// </summary>
        /// <param name="configuration"></param>
        public NewsContext(IConfiguration configuration)
        {
            ///Initialize client with connection string
            mongoClient = new MongoClient(configuration.GetSection("MongoDB").GetSection("ConnectionString").Value);
            ///Initialize database using database name
            mongoDb = mongoClient.GetDatabase(configuration.GetSection("MongoDB").GetSection("NewsDatabase").Value);
        }

        /// <summary>
        /// Represents the News Collection in database
        /// </summary>
        public IMongoCollection<UserNews> News => mongoDb.GetCollection<UserNews>("userNews");
    }
}
