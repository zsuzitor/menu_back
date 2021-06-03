

using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public class PlanitUserReturn
    {
        [JsonPropertyName("id")]
        public string UserIdentifier { get; set; }//signalRUserId
        [JsonPropertyName("is_admin")]

        public bool IsAdmin { get; set; }//enum?
        [JsonPropertyName("name")]

        public string Name { get; set; }
        [JsonPropertyName("vote")]

        public int? Vote { get; set; }
        [JsonPropertyName("has_vote")]

        public bool HasVote { get; set; }


        public PlanitUserReturn(PlanitUser obj)
        {
            UserIdentifier = obj.UserIdentifier;
            IsAdmin = obj.Role.Contains("Admin");
            Name = obj.Name;
            Vote = obj.Vote;
            HasVote = obj.HasVote;
        }
    }
}
