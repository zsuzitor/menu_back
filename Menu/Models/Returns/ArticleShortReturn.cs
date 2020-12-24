﻿using Menu.Models.Poco;
using Menu.Models.Returns.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Menu.Models.Returns
{
    public class ArticleShortReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
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
        [JsonPropertyName("main_image_path")]
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