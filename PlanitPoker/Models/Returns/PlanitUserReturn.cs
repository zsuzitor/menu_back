

using System.Collections.Generic;
using System.Text.Json.Serialization;
using PlanitPoker.Models.Entity;

namespace PlanitPoker.Models.Returns
{
    public sealed class PlanitUserReturn
    {
        [JsonPropertyName("id")]
        public string UserIdentifier { get; set; }
        
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
        [JsonPropertyName("name")]

        public string Name { get; set; }
        [JsonPropertyName("vote")]

        public string Vote { get; set; }
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
