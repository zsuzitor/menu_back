using BO.Models.WordsCardsApp.DAL.Domain;
using Menu.Models.Returns.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types.WordsCardsApp
{

    public class WordCardReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj is WordCard objTyped)
            {
                return new WordCardReturn(objTyped);
            }

            if (obj is IEnumerable<WordCard> objTypedList)
            {
                return objTypedList?.Select(x => new WordCardReturn(x)).ToList();
            }

            return obj;
        }
    }


    public class WordCardReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("image_path")]
        public string ImagePath { get; set; }

        [JsonPropertyName("word")]
        public string Word { get; set; }
        [JsonPropertyName("word_answer")]
        public string WordAnswer { get; set; }//не хотел завязываться на слово "перевод"
        [JsonPropertyName("hided")]
        public bool Hided { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }


        [JsonPropertyName("lists")]
        public List<WordCardWordListReturn> Lists { get; set; }

        public WordCardReturn(WordCard obj)
        {
            Id = obj.Id;
            ImagePath = obj.ImagePath;
            Word = obj.Word;
            WordAnswer = obj.WordAnswer;
            Hided = obj.Hided;
            Description = obj.Description;
            UserId = obj.UserId;
            Lists = new List<WordCardWordListReturn>();
            if (obj.WordCardWordList != null)
            {
                foreach (var lst in obj.WordCardWordList)
                {
                    Lists.Add(new WordCardWordListReturn(lst));
                }
            }
        }

    }


   
}
