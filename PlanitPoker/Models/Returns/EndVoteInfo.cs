using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{

    public class EndVoteUserInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("vote")]
        public int Vote { get; set; }
    }

    public class EndVoteInfo
    {
        [JsonPropertyName("users_info")]
        public List<EndVoteUserInfo> UsersInfo { get; set; }
        [JsonPropertyName("average_vote")]
        public double Average { get; set; }
        [JsonPropertyName("min_vote")]
        public int MinVote { get; set; }
        [JsonPropertyName("max_vote")]
        public int MaxVote { get; set; }
    }
}
