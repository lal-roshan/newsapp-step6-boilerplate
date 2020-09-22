using System;

namespace UserService.Exceptions
{
    /// <summary>
    /// Exception to be thrown if a user already exists
    /// </summary>
    public class UserAlreadyExistsException : ApplicationException
    {
        public UserAlreadyExistsException() { }
        public UserAlreadyExistsException(string message) : base(message) { }
    }
}
