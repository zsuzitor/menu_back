using PlanitPoker.Models.Enums;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public class Notify
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("status")]
        public NotyfyStatus Status { get; set; }
    }
}
