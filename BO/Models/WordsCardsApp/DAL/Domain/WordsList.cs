using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

namespace BO.Models.WordsCardsApp.DAL.Domain
{
   public class WordsList : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Title { get; set; }


        public long? UserId { get; set; }
        public User User { get; set; }

        public List<WordCardWordList> WordCardWordList { get; set; }

        public WordsList()
        {
            WordCardWordList = new List<WordCardWordList>();
        }
    }
}
