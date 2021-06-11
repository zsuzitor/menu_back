
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public class RoomInfoReturn
    {
        [JsonPropertyName("room")]
        public StoredRoomReturn Room { get; set; }
        [JsonPropertyName("end_vote_info")]
        public EndVoteInfo EndVoteInfo { get; set; }
    }
}
