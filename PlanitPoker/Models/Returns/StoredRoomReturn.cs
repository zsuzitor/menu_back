

using PlanitPoker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public class StoredRoomReturn
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("die_date")]
        public DateTime DieDate { get; set; }
        [JsonPropertyName("users")]
        public List<PlanitUserReturn> Users { get; set; }
        [JsonPropertyName("status")]
        public RoomSatus Status { get; set; }


        public StoredRoomReturn()
        {
            Users = new List<PlanitUserReturn>();
        }

        public StoredRoomReturn(StoredRoom obj) : this()
        {
            if (obj == null)
            {
                return;
            }

            Name = obj.Name;
            DieDate = obj.DieDate;
            Status = obj.Status;

            if (obj.Users != null)
            {
                Users = obj.Users.Select(x => new PlanitUserReturn(x)).ToList();

            }
        }
    }
}
