using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace UserService.Models
{
    /// <summary>
    /// Class fascilitating communication with database
    /// </summary>
    public class UserContext
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
        public UserContext(IConfiguration configuration)
        {
            ///Initialize client with connection string
            mongoClient = new MongoClient(configuration.GetSection("MongoDB").GetSection("ConnectionString").Value);
            ///Initialize database using database name
            mongoDb = mongoClient.GetDatabase(configuration.GetSection("MongoDB").GetSection("UserDatabase").Value);
        }

        /// <summary>
        /// Represents the Users Collection in database
        /// </summary>
        public IMongoCollection<UserProfile> Users => mongoDb.GetCollection<UserProfile>("users");
    }
}
