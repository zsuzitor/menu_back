using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanitPoker.Models
{
    public class PlanitUser//:ICloneable
    {
        public string UserIdentifier { get; set; }//signalRUserId
        public List<string> Role { get; set; }//enum?
        public string Name { get; set; }
        public int? Vote { get; set; }
        ////не актуально на стороне сервера, проставлятся только для отправки на ui. todo можно вынести в отдельную модель
        public bool HasVote { get; set; }

        public bool IsAdmin
        {
            get
            {
                return Role.Any(x => x == Consts.Roles.Creator || x == Consts.Roles.Admin);
            }
        }

        public bool CanVote
        {
            get
            {
                return !Role.Any(x => x == Consts.Roles.Observer);
            }
        }

        public PlanitUser Clone()
        {
            var res = (PlanitUser)this.MemberwiseClone();
            res.Role = new List<string>(this.Role);
            return res;
        }


    }
}
