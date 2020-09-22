using System;

namespace UserService.Exceptions
{
    /// <summary>
    /// Exception to be thrown when user a requested user is not found
    /// </summary>
    public class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException() { }
        public UserNotFoundException(string message) : base(message) { }
    }
}
