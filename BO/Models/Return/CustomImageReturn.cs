using System.Text.Json.Serialization;
using BO.Models.DAL.Domain;

namespace BO.Models.Return
{
    public sealed class CustomImageReturn
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
