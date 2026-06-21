using System;
using System.Text.Json.Serialization;
using PlaningPoker.Models.Entity;

namespace PlaningPoker.Models.Returns
{
    public sealed class StoryReturn
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("vote")]
        public decimal Vote { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
        [JsonPropertyName("current_session")]
        public bool CurrentSession { get; set; }


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
            CurrentSession = obj.CurrentSession;
        }
    }
}
