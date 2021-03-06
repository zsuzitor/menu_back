﻿

using BO.Models.DAL;
using BO.Models.DAL.Domain;

namespace BO.Models.PlaningPoker.DAL
{
    public class PlaningRoomUserDal : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Roles { get; set; }
        public string Name { get; set; }


        public long MainAppUserId { get; set; }
        public User MainAppUser { get; set; }

        

        public long RoomId { get; set; }
        public PlaningRoomDal Room { get; set; }
    }
}
