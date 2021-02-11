

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordsCardsApp.BO.Input;

namespace Menu.Models.InputModels.WordsCardsApp
{
    public class WordCardInputModelApi
    {
        [BindProperty(Name = "id", SupportsGet = false)]
        public long? Id { get; set; }
        [BindProperty(Name = "word", SupportsGet = false)]
        public string Word { get; set; }
        [BindProperty(Name = "word_answer", SupportsGet = false)]
        public string WordAnswer { get; set; }
        [BindProperty(Name = "description", SupportsGet = false)]
        public string Description { get; set; }

        /// <summary>
        /// we need CLEAR main image(not change, clear!)
        /// </summary>
        [BindProperty(Name = "delete_main_image", SupportsGet = false)]
        public bool? DeleteMainImage { get; set; }
        [BindProperty(Name = "main_image_new", SupportsGet = false)]
        public IFormFile MainImageNew { get; set; }




        public WordCardInputModelApi()
        {
            DeleteMainImage = false;
        }

        public WordCardInputModel GetModel()
        {
            return new WordCardInputModel()
            {
                Id = this.Id,
                Word = this.Word,
                WordAnswer = this.WordAnswer,
                Description = this.Description,
                MainImageNew = this.MainImageNew,
                DeleteMainImage = this.DeleteMainImage,
            };
        }
    }
}
