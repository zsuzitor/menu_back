

using System.Collections.Generic;

namespace Common.Models.Error
{
    public class ErrorObject
    {
        public int? Status { get; set; }
        public List<OneError> Errors { get; set; }

        public ErrorObject()
        {
            Errors = new List<OneError>();
        }
    }
}
