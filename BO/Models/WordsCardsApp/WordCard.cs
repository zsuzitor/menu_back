using BO.Models.DAL.Domain;

namespace BO.Models.WordsCardsApp.DAL.Domain
{
    public class WordCard
    {
        public long Id { get; set; }
        public string ImagePath { get; set; }

        public string Word { get; set; }
        public string WordAnswer { get; set; }//не хотел завязываться на слово "перевод"
        public bool Hided { get; set; }
        public string Description { get; set; }


        public long? UserId { get; set; }
        public User User { get; set; }
    }
}
