using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NewsService.Models
{
    /// <summary>
    /// Class representing a news entity of user
    /// </summary>
    public class News
    {
        [BsonId]
        public int NewsId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishedAt { get; set; }

        public string Url { get; set; }

        public string UrlToImage { get; set; }

        public Reminder Reminder { get; set; }
    }
}
