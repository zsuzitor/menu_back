

using Common.Models.Exceptions;
using System.Collections.Generic;

namespace Common.Models.Error
{
    public class OneError
    {
        public string Key { get; set; }
        public List<string> Errors { get; set; }

        public OneError(string key,string body)
        {
            Key = key;
            Errors = new List<string>() { body };
        }

        public OneError(string key, List<string> body)
        {
            Key = key;
            Errors = body;
        }


        public OneError(SomeCustomException exc)
        {
            Key = exc.Message;
            Errors = new List<string>() { exc.Body };
        }
    }
}
