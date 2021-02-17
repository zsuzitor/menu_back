using BO.Models.DAL;


namespace BO.Models.WordsCardsApp.DAL.Domain
{
    public class WordCardWordList : IDomainRecord<long>
    {
        public long Id { get; set; }
        public long WordCardId { get; set; }
        public WordCard WordCard { get; set; }

        public long WordsListId { get; set; }
        public WordsList WordsList { get; set; }
    }
}
