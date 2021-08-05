using BO.Models.WordsCardsApp.DAL.Domain;
using WEB.Common.Models.Returns.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Common.Models;

namespace Menu.Models.Returns.Types.WordsCardsApp
{
    public class WordListReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is WordsList objTyped)
            {
                return new WordCardListReturn(objTyped);
            }

            if (obj is IEnumerable<WordsList> objTypedList)
            {
                return objTypedList?.Select(x => new WordCardListReturn(x)).ToList();
            }

            return obj;
        }
    }


    public class WordCardListReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }

       

        public WordCardListReturn(WordsList obj)
        {
            Id = obj.Id;
            Title = obj.Title;
        }

    }
}
