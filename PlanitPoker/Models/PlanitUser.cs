using System;
using System.Collections.Generic;
using System.Text;

namespace PlanitPoker.Models
{
    public class PlanitUser
    {
        public string UserIdentifier { get; set; }//signalRUserId
        public List<string> Role { get; set; }//enum?
        public string Name { get; set; }
        public int Vote { get; set; }


    }
}
