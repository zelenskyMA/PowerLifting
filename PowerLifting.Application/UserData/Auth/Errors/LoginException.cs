﻿namespace PowerLifting.Application.UserData.Auth.Errors
{
    [Serializable]
    public class LoginException : Exception
    {
        public LoginException() { }

        public LoginException(string message) : base(message) { }

        public LoginException(string message, Exception innerException) : base(message, innerException) { }
    }
}
