using System;

namespace Menu.Models.Exceptions
{
    public class SomeCustomException : Exception
    {
        public string Body { get; set; }


        //message - key


        public SomeCustomException()
        {

        }

        /// <summary>
        /// search in static error list, if do not searched write in body
        /// </summary>
        /// <param name="message"></param>
        public SomeCustomException(string message) : base(message)
        {

        }

        public SomeCustomException(string key, string body) : base(key)
        {
            Body = body;
        }
    }
}
