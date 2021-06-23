using System;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public class StoryReturn
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("vote")]
        public double Vote { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("completed")]
        public bool Completed { get; set; }


        public StoryReturn()
        {

        }

        public StoryReturn(Story obj) : this()
        {
            Id = obj.Id;
            Name = obj.Name;
            Description = obj.Description;
            Vote = obj.Vote;
            Date = obj.Date;
            Completed = obj.Completed;
        }
    }
}
