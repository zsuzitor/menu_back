using System;

namespace Common.Models.Exceptions
{
    public class SomeCustomException : Exception
    {
        /// <summary>
        /// если не задано то key пойдет как body на фронт, если не будет найден в обработчике ошибок
        /// </summary>
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

        public SomeCustomException(string message, Exception innerExcp) : base(message, innerExcp)
        {

        }

        public SomeCustomException(string key, string body) : base(key)
        {
            Body = body;
        }

        public SomeCustomException(string key, string body, Exception innerExcp) : base(key, innerExcp)
        {
            Body = body;
        }
    }
}
