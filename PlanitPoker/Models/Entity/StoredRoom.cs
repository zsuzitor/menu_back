using PlanitPoker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanitPoker.Models.Entity
{
    public sealed class StoredRoom
    {
        public long? Id { get; set; }//только для уже существующих в бд записей
        public string Name { get; set; }
        public string Password { get; set; }


        public DateTime DieDate { get; set; }
        public List<PlanitUser> Users { get; set; }
        public RoomSatus Status { get; set; }

        public List<Story> Stories { get; set; }
        public string CurrentStoryId { get; set; }
        [Obsolete]
        public bool OldStoriesAreLoaded { get; set; }
        //public long StoryForAddMaxTmpId { get; set; }
        public long TotalNotActualStoriesCount { get; set; }
        public List<string> Cards { get; set; }


        public StoredRoom()
        {
            Status = RoomSatus.CloseVote;
            Users = new List<PlanitUser>();
            Stories = new List<Story>();
            DieDate = DateTime.Now.AddHours(Consts.DefaultHourRoomAlive);
            CurrentStoryId = string.Empty;
            Cards = new List<string>();
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
