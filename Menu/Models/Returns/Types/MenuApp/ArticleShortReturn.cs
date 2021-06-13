
using WEB.Common.Models.Returns.Interfaces;
using MenuApp.Models.BO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types.MenuApp
{
    public class ArticleShortReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is ArticleShort objTyped)
            {
                return new ArticleShortReturn(objTyped);
            }

            if (obj is IEnumerable<ArticleShort> objTypedList)
            {
                return objTypedList?.Select(x=> new ArticleShortReturn(x)).ToList();
            }

            return obj;
        }
    }

    public class ArticleShortReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("image")]
        public string MainImagePath { get; set; }
        [JsonPropertyName("followed")]
        public bool Followed { get; set; }

        public ArticleShortReturn(ArticleShort obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            Body = obj.Body;
            MainImagePath = obj.MainImagePath;
            Followed = obj.Followed;
        }
    }
}
