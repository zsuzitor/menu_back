

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public class PlanitUserReturn
    {
        [JsonPropertyName("id")]
        public string UserIdentifier { get; set; }
        
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
        [JsonPropertyName("name")]

        public string Name { get; set; }
        [JsonPropertyName("vote")]

        public int? Vote { get; set; }
        [JsonPropertyName("has_vote")]

        public bool HasVote { get; set; }


        public PlanitUserReturn(PlanitUser obj)
        {
            if (obj == null)
            {
                return;
            }

            UserIdentifier = obj.PlaningAppUserId;
            Roles = obj.Role;//obj.IsAdmin,//obj.Role.Contains();
            Name = obj.Name;
            Vote = obj.Vote;
            HasVote = obj.HasVote;
        }
    }
}
