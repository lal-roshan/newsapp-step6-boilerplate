using System;

namespace ReminderService.Exceptions
{
    public class ReminderAlreadyExistsException : ApplicationException
    {
        public ReminderAlreadyExistsException() { }
        public ReminderAlreadyExistsException(string message) : base(message) { }
    }
}
