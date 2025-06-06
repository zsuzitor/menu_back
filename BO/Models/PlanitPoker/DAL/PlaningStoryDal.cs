﻿using BO.Models.DAL;
using System;

namespace BO.Models.PlaningPoker.DAL
{
    public sealed class PlaningStoryDal : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Vote { get; set; }
        public DateTime Date { get; set; }
        public bool Completed { get; set; }

        public long RoomId { get; set; }
        public PlaningRoomDal Room { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
