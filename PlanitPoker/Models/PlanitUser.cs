﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanitPoker.Models
{
    public class PlanitUser//:ICloneable
    {
        public long? MainAppUserId { get; set; }//пользователь авторизован в основной апе
        private string _planingAppUserId = "";
        public string PlaningAppUserId //id пользователя в планинге
        {
            get
            {
                if (MainAppUserId!=null)
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
                if (MainAppUserId!=null)
                {
                    _planingAppUserId = value;
                }
            }
        }
        public string UserConnectionId { get; set; }//signalRUserId
        public List<string> Role { get; set; }//enum?
        public string Name { get; set; }
        public int? Vote { get; set; }
        ////не актуально на стороне сервера, проставлятся только для отправки на ui. todo можно вынести в отдельную модель
        public bool HasVote { get; set; }

        //public string Login { get; set; }//если пользак авторизован полностью в мейн апе

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


        public PlanitUser()
        {
            Role = new List<string>();
        }


        public PlanitUser Clone()
        {
            var res = (PlanitUser)this.MemberwiseClone();
            res.Role = new List<string>(this.Role);
            return res;
        }


    }
}
