using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models.Exceptions
{
    public class SomeCustomNotFoundException : SomeCustomException
    {
        public SomeCustomNotFoundException()
        {

        }

        /// <summary>
        /// search in static error list, if do not searched write in body
        /// </summary>
        /// <param name="message"></param>
        public SomeCustomNotFoundException(string message) : base(message)
        {

        }

        public SomeCustomNotFoundException(string message, Exception innerExcp) : base(message, innerExcp)
        {

        }

        public SomeCustomNotFoundException(string key, string body) : base(key)
        {
        }

        public SomeCustomNotFoundException(string key, string body, Exception innerExcp) : base(key, innerExcp)
        {
        }
    }
}
