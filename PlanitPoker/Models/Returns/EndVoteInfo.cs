using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{

    public sealed class EndVoteUserInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("vote")]
        public string Vote { get; set; }
    }

    public class EndVoteInfo
    {
        [JsonPropertyName("users_info")]
        public List<EndVoteUserInfo> UsersInfo { get; set; }
        [JsonPropertyName("average_vote")]
        public decimal Average { get; set; }
        [JsonPropertyName("min_vote")]
        public decimal MinVote { get; set; }

        [JsonPropertyName("max_vote")]
        public decimal MaxVote { get; set; }
    }
}
