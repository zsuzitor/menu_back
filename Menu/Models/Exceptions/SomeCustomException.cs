using System;

namespace Menu.Models.Exceptions
{
    public class SomeCustomException : Exception
    {
        public string Body { get; set; }


        public SomeCustomException()
        {

        }

        public SomeCustomException(string message) :base(message)
        {

        }

        public SomeCustomException(string key,string body) : base(key)
        {
            Body = body;
        }
    }
}
