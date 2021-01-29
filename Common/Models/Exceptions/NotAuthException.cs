using System;

namespace Common.Models.Exceptions
{
    /// <summary>
    /// user not auth
    /// </summary>
    public class NotAuthException : Exception
    {
        public NotAuthException() : base()
        {

        }

        public NotAuthException(string message) : base(message)
        {

        }

        public NotAuthException(string message, Exception innerExc) : base(message, innerExc)
        {

        }
    }
}
