using BO.Models.MenuApp.DAL.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using BO.Models.Return;
using Common.Models;
using Common.Models.Return;

namespace Menu.Models.Returns.Types.MenuApp
{


    public class ArticleReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is Article objTyped)
            {
                return new ArticleReturn(objTyped);
            }

            if (obj is IEnumerable<Article> objTypedList)
            {
                return objTypedList?.Select(x => new ArticleReturn(x)).ToList();
            }

            return obj;
        }

        
    }

    //public interface IConv<T,V>
    //{
    //    V Convert(T obj);
    //}

    public class ArticleReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("main_image_path")]
        public string MainImagePath { get; set; }


        [JsonPropertyName("followed")]
        public bool Followed { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("additional_images")]
        public List<CustomImageReturn> AdditionalImages { get; set; }

        public ArticleReturn(Article obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            Body = obj.Body;
            MainImagePath = obj.MainImagePath;
            Followed = obj.Followed;
            UserId = obj.UserId;
            AdditionalImages = obj.AdditionalImages?.Select(x => new CustomImageReturn(x)).ToList();
        }
    }
}
