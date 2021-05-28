using PlanitPoker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PlanitPoker.Models
{
    public class Room
    {
        //public long Id { get; set; }
        public string Name { get; set; }
        
        

        public DateTime DieDate { get; set; }
        public PlanitUser Users { get; set; }
        public RoomSatus Status { get; set; }


        public object Lock { get; set; }
        public ReaderWriterLock RWL = new ReaderWriterLock();

    }
}
