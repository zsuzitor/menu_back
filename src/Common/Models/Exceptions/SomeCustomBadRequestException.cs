using System;

namespace Common.Models.Exceptions
{
    public class SomeCustomBadRequestException : SomeCustomException
    {
        public SomeCustomBadRequestException()
        {

        }

        /// <summary>
        /// search in static error list, if do not searched write in body
        /// </summary>
        /// <param name="message"></param>
        public SomeCustomBadRequestException(string message) : base(message)
        {

        }

        public SomeCustomBadRequestException(string message, Exception innerExcp) : base(message, innerExcp)
        {

        }

        public SomeCustomBadRequestException(string key, string body) : base(key, body)
        {
        }

        public SomeCustomBadRequestException(string key, string body, Exception innerExcp) : base(key, body, innerExcp)
        {
        }
    }
}
