using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

namespace BO.Models.WordsCardsApp.DAL.Domain
{
    public class WordCard : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string ImagePath { get; set; }

        public string Word { get; set; }
        public string WordAnswer { get; set; }//не хотел завязываться на слово "перевод"
        public bool Hided { get; set; }
        public string Description { get; set; }


        public long? UserId { get; set; }
        public User User { get; set; }


        public List<WordCardWordList> WordCardWordList { get; set; }

        public WordCard()
        {
            WordCardWordList = new List<WordCardWordList>();
        }
    }
}
