using Microsoft.AspNetCore.Http;

namespace WordsCardsApp.BO.Input
{
    public sealed class WordCardInputModel
    {
        public long? Id { get; set; }//актуально только для редактирования
        public string Word { get; set; }
        public string WordAnswer { get; set; }
        public string Description { get; set; }
        public long? ListId { get; set; }//актуально только для создания

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
