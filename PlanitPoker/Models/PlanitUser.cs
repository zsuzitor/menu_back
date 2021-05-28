using System;
using System.Collections.Generic;
using System.Text;

namespace PlanitPoker.Models
{
    public class PlanitUser//:ICloneable
    {
        public string UserIdentifier { get; set; }//signalRUserId
        public List<string> Role { get; set; }//enum?
        public string Name { get; set; }
        public int? Vote { get; set; }

        public PlanitUser Clone()
        {
            var res = (PlanitUser)this.MemberwiseClone();
            res.Role = new List<string>(this.Role);
            return res;
        }


    }
}
