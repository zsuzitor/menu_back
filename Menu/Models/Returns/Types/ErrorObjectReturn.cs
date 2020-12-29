using Menu.Models.Error;
using Menu.Models.Returns.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types
{
    public class ErrorObjectReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj is ErrorObject objTyped)
            {
                return new ErrorObjectReturn(objTyped);
            }

            return obj;
        }
    }

    public class ErrorObjectReturn
    {
        [JsonPropertyName("status")]
        public int? Status { get; set; }
        [JsonPropertyName("errors")]
        public List<OneErrorReturn> Errors { get; set; }

        public ErrorObjectReturn(ErrorObject obj)
        {
            Status = obj.Status;
            Errors = obj.Errors?.Select(x => new OneErrorReturn(x)).ToList();
        }
    }
}
