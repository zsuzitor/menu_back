using PlanitPoker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<Story> Stories { get; set; }
        public string CurrentStoryId { get; set; }
        //public long StoryForAddMaxTmpId { get; set; }

        public StoredRoom()
        {
            Status = RoomSatus.CloseVote;
            Users = new List<PlanitUser>();
            Stories = new List<Story>();
            DieDate = DateTime.Now.AddHours(2);
            CurrentStoryId = "";
        }

        public StoredRoom(string name, string password) : this()
        {
            Name = name;
            Password = password;
        }


        public StoredRoom Clone()
        {
            var res = (StoredRoom)this.MemberwiseClone();
            res.Users = Users.Select(x => x.Clone()).ToList();
            res.Stories = Stories.Select(x=>x.Clone()).ToList();
            return res;
        }


    }
}
