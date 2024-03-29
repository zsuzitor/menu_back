﻿using BO.Models.PlaningPoker.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace PlanitPoker.Models.Entity
{
    public sealed class PlanitUser//:ICloneable
    {
        public long? MainAppUserId { get; set; }//пользователь авторизован в основной апе
        private string _planingAppUserId = null;
        public string PlaningAppUserId //id пользователя в планинге
        {
            get
            {
                if (MainAppUserId == null)
                {
                    return _planingAppUserId;
                }
                else
                {
                    return MainAppUserId.ToString();
                }
            }
            set
            {
                if (MainAppUserId == null)
                {
                    _planingAppUserId = value;
                }
            }
        }

        public string UserConnectionId { get; set; }//signalRUserId
        public List<string> Role { get; set; }//enum?
        public string Name { get; set; }
        public string Vote { get; set; }
        ////не актуально на стороне сервера, проставлятся только для отправки на ui. todo можно вынести в отдельную модель
        private bool _hasVote;
        public bool HasVote { get => (_hasVote ? _hasVote: Vote != null);
            set { _hasVote = value; } }
        public string ImageLink { get; set; }


        //public string Login { get; set; }//если пользак авторизован полностью в мейн апе

        public bool IsAdmin
        {
            get
            {
                return Role.Any(x => x == Constants.Roles.Creator || x == Constants.Roles.Admin);
            }
        }

        public bool CanVote//должно быть синхронно с фронтом
        {
            get
            {
                return !Role.Any(x => x == Constants.Roles.Observer);
            }
        }


        public PlanitUser()
        {
            Role = new List<string>();
        }

        public PlanitUser(PlanitUser user)
        {
            MainAppUserId = user.MainAppUserId;
            PlaningAppUserId = user.PlaningAppUserId;
            UserConnectionId = user.UserConnectionId;
            Role = user.Role.ToList();
            Name = user.Name;
            Vote = user.Vote;
            HasVote = user.HasVote;
            ImageLink = user.ImageLink;
        }


        public PlanitUser Clone()
        {
            var res = (PlanitUser)this.MemberwiseClone();
            res.Role = new List<string>(this.Role);
            return res;
        }

        public PlaningRoomUserDal ToDbObject(long roomId)
        {
            if (MainAppUserId == null)
            {
                return null;
            }

            var forDb = new PlaningRoomUserDal();
            forDb.MainAppUserId = (long)MainAppUserId;
            forDb.Name = Name;
            //forDb.Roles = string.Join(",", Role);
            forDb.Roles = JsonSerializer.Serialize(Role);
            forDb.RoomId = roomId;

            return forDb;
        }

        public void FromDbObject(PlaningRoomUserDal obj)
        {
            Name = obj.Name;
            MainAppUserId = obj.MainAppUserId;
            //Role = obj.Roles.Split(',').ToList();
            Role = JsonSerializer.Deserialize<List<string>>(obj.Roles);
        }
    }
}
