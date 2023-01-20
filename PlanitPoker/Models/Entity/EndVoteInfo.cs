using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Entity
{

    public sealed class EndVoteUserInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("vote")]
        public string Vote { get; set; }

        public EndVoteUserInfo()
        {

        }

        public EndVoteUserInfo(EndVoteUserInfo obj)
        {
            Id = obj.Id;
            Vote = obj.Vote;
        }
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

        public EndVoteInfo()
        {
            UsersInfo = new List<EndVoteUserInfo>();
        }


        public EndVoteInfo(EndVoteInfo obj) :this()
        {
            UsersInfo = obj.UsersInfo.Select(x => new EndVoteUserInfo(x)).ToList();
            Average = obj.Average;
            MinVote = obj.MinVote;
            MaxVote = obj.MaxVote;
        }
    }
}
