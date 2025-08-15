

using BO.Models.WordsCardsApp.DAL.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Common.Models;

namespace Menu.Models.WordsCardsApp.Returns
{
    public sealed class WordCardWordListReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is WordCardWordList objTyped)
            {
                return new WordCardWordListReturn(objTyped);
            }

            if (obj is IEnumerable<WordCardWordList> objTypedList)
            {
                return objTypedList?.Select(x => new WordCardWordListReturn(x)).ToList();
            }

            return obj;
        }
    }



    public class WordCardWordListReturn
    {
        [JsonPropertyName("id_list")]
        public long IdList { get; set; }

        [JsonPropertyName("id_word")]
        public long IdWord { get; set; }

        public WordCardWordListReturn(WordCardWordList obj)
        {
            IdList = obj.WordsListId;
            IdWord = obj.WordCardId;
        }
    }
}
