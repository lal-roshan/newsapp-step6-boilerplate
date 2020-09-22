using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace NewsService.Models
{
    /// <summary>
    /// Class representing the news document
    /// </summary>
    public class UserNews
    {
        [BsonId]
        public string UserId { get; set; }

        public List<News> NewsList { get; set; }

    }
}
