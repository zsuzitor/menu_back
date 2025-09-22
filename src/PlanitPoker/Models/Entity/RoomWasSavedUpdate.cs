using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Entity
{
    public class RoomWasSavedUpdate
    {

        public class StoryMapping
        {
            [JsonPropertyName("old_id")]
            public string OldId { get; set; }

            [JsonPropertyName("new_id")]
            public long NewId { get; set; }

            public StoryMapping(string oldId, long newId)
            {
                OldId = oldId;
                NewId = newId;
            }
        }

        [JsonPropertyName("stories_mapping")]
        public List<StoryMapping> StoriesMapping { get; set; }
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        public RoomWasSavedUpdate()
        {
            StoriesMapping = new List<StoryMapping>();
        }

        public RoomWasSavedUpdate(List<StoryMapping> storiesMapping, bool success)
        {
            StoriesMapping = storiesMapping;
            Success = success;
        }

    }

    
}
