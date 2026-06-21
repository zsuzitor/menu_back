

using System.Collections.Generic;
using System.Text.Json.Serialization;
using PlaningPoker.Models.Entity;

namespace PlaningPoker.Models.Returns
{
    public sealed class PlaningUserReturn
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

        [JsonPropertyName("image_link")]
        public string ImageLink { get; set; }



        public PlaningUserReturn(PlaningUser obj)
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
            ImageLink = obj.ImageLink;
        }
    }
}
