﻿using System;

namespace AuthenticationService.Exceptions
{
    public class UserNotFoundException:ApplicationException
    {
        public UserNotFoundException() { }
        public UserNotFoundException(string message) : base(message) { }
    }
}
