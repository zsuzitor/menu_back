

using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System;

namespace BO.Models.PlaningPoker.DAL
{
    public sealed class PlaningRoomUserDal : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Roles { get; set; }
        public string Name { get; set; }
        //public DateTime LastActive { get; set; }


        public long MainAppUserId { get; set; }
        public User MainAppUser { get; set; }

        

        public long RoomId { get; set; }
        public PlaningRoomDal Room { get; set; }
    }
}
