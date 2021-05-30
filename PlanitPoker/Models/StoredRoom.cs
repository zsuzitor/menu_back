using PlanitPoker.Models.Enums;
using System;
using System.Collections.Generic;

namespace PlanitPoker.Models
{
    public class StoredRoom
    {
        //public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }//TODO потом зашифровать


        public DateTime DieDate { get; set; }
        public List<PlanitUser> Users { get; set; }
        public RoomSatus Status { get; set; }

        public StoredRoom()
        {
            Status = RoomSatus.CloseVote;
            Users = new List<PlanitUser>();
            DieDate = DateTime.Now.AddHours(1);
        }

        public StoredRoom(string name, string password) : this()
        {
            Name = name;
            Password = password;
        }


    }
}
