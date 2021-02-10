using Microsoft.AspNetCore.Http;

namespace WordsCardsApp.BO.Input
{
    public class WordCardInputModel
    {
        public long? Id { get; set; }
        public string Word { get; set; }
        public string WordAnswer { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// we need CLEAR main image(not change, clear!)
        /// </summary>
        public bool? DeleteMainImage { get; set; }
        public IFormFile MainImageNew { get; set; }

        


        public WordCardInputModel()
        {
            DeleteMainImage = false;
        }
    }
}
