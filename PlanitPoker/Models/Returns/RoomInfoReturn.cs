
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public sealed class RoomInfoReturn
    {
        [JsonPropertyName("room")]
        public StoredRoomReturn Room { get; set; }
        [JsonPropertyName("end_vote_info")]
        public EndVoteInfo EndVoteInfo { get; set; }

        
        //[JsonPropertyName("die_room_time")]
        //public DateTime DieRoomTime { get; set; }
    }
}
