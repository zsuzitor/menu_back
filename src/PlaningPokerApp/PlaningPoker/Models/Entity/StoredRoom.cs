using BO.Models.DAL.Domain;
using PlaningPoker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaningPoker.Models.Entity
{
    public sealed class StoredRoom
    {
        public long? Id { get; set; }//только для уже существующих в бд записей
        public string Name { get; set; }
        public string Password { get; set; }
        public CustomFile Image { get; set; }//todo можно создать укороченный тип


        public DateTime DieDate { get; set; }
        public List<PlaningUser> Users { get; set; }
        public RoomSatus Status { get; set; }

        public List<Story> Stories { get; set; }
        public string CurrentStoryId { get; set; }
        [Obsolete]
        public bool OldStoriesAreLoaded { get; set; }
        //public long StoryForAddMaxTmpId { get; set; }
        public long TotalNotActualStoriesCount { get; set; }
        public List<string> Cards { get; set; }

        public EndVoteInfo EndVoteInfo { get; set; }


        public StoredRoom()
        {
            Status = RoomSatus.CloseVote;
            Users = new List<PlaningUser>();
            Stories = new List<Story>();
            CurrentStoryId = string.Empty;
            Cards = new List<string>();
            EndVoteInfo = new EndVoteInfo();
        }

        public StoredRoom(string name, string password) : this()
        {
            Name = name;
            Password = password;
        }

        public void SetDieDate(DateTime dt)
        {
            DieDate = dt.AddHours(Constants.DefaultHourRoomAlive);
        }


        public StoredRoom Clone()
        {
            var res = (StoredRoom)this.MemberwiseClone();
            res.Users = Users.Select(x => x.Clone()).ToList();
            res.Stories = Stories.Select(x=>x.Clone()).ToList();
            res.Cards = Cards.Select(x => x).ToList();
            return res;
        }


    }
}
