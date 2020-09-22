using System;

namespace ReminderService.Exceptions
{
    public class NoReminderFoundException : ApplicationException
    {
        public NoReminderFoundException() { }
        public NoReminderFoundException(string message) : base(message) { }
    }
}
