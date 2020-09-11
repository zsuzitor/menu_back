using Menu.Models.Error;
using Menu.Models.Returns.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menu.Models.Returns
{
    public class ErrorObjectFactory : IReturnObjectFactory
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
        public int? status { get; set; }
        public List<OneErrorReturn> errors { get; set; }

        public ErrorObjectReturn(ErrorObject obj)
        {
            status = obj.Status;
            errors = obj.Errors?.Select(x => new OneErrorReturn(x)).ToList();
        }
    }
}
