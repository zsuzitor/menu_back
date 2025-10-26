using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models.Exceptions
{
    public class SomeCustomNotAllowedException : SomeCustomException
    {
        public SomeCustomNotAllowedException()
        {

        }

        /// <summary>
        /// search in static error list, if do not searched write in body
        /// </summary>
        /// <param name="message"></param>
        public SomeCustomNotAllowedException(string message) : base(message)
        {

        }

        public SomeCustomNotAllowedException(string message, Exception innerExcp) : base(message, innerExcp)
        {

        }

        public SomeCustomNotAllowedException(string key, string body) : base(key)
        {
        }

        public SomeCustomNotAllowedException(string key, string body, Exception innerExcp) : base(key, innerExcp)
        {
        }
    }
}
