namespace AuthenticationService.Models
{
    /// <summary>
    /// Class representing an entity in Users table
    /// </summary>
    public class User
    {
        public string UserId { get; set; }

        public string Password { get; set; }
    }
}
