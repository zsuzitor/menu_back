using Menu.Models.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menu.Models.Returns
{
    public class OneErrorReturn
    {
        public string key { get; set; }
        public List<string> errors { get; set; }

        public OneErrorReturn(OneError obj)
        {
            key = obj.Key;
            errors = obj.Errors;
        }
    }
}
