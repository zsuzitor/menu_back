using BO.Models.DAL.Domain;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types
{
    public class CustomImageReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("article_id")]
        public long? ArticleId { get; set; }

        public CustomImageReturn(CustomImage obj)
        {
            Id = obj?.Id?? 0;
            Path = obj?.Path;
            ArticleId = obj?.ArticleId;
        }
    }
}
