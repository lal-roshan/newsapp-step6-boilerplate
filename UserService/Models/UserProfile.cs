using MongoDB.Bson.Serialization.Attributes;
using System;
namespace UserService.Models
{
    /// <summary>
    /// Class representing User Profile Document
    /// </summary>
    public class UserProfile
    {
        [BsonId]
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
