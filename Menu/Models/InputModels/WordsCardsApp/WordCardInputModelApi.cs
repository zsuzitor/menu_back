

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using WordsCardsApp.BO.Input;

namespace Menu.Models.InputModels.WordsCardsApp
{
    public sealed class WordCardInputModelApi
    {
        [BindProperty(Name = "id", SupportsGet = false)]
        public long? Id { get; set; }

        [BindProperty(Name = "word", SupportsGet = false)]
        [Required]
        public string Word { get; set; }
        [BindProperty(Name = "word_answer", SupportsGet = false)]
        [Required]
        public string WordAnswer { get; set; }
        [BindProperty(Name = "description", SupportsGet = false)]
        public string Description { get; set; }

        [BindProperty(Name = "list_id", SupportsGet = false)]
        public long? ListId { get; set; }

        /// <summary>
        /// we need CLEAR main image(not change, clear!)
        /// </summary>
        [BindProperty(Name = "delete_main_image", SupportsGet = false)]
        public bool? DeleteMainImage { get; set; }
        [BindProperty(Name = "main_image_new", SupportsGet = false)]
        public IFormFile MainImageNew { get; set; }


        public void Validate(Func<string, string> strValidator, Action<IFormFile, ModelStateDictionary> fileValidator, ModelStateDictionary modelState)
        {
            if (string.IsNullOrWhiteSpace(Word))
            {
                Word = string.Empty;
            }
            else
            {
                Word = strValidator(Word.Trim());
            }

            
            if (string.IsNullOrWhiteSpace(WordAnswer))
            {
                WordAnswer = string.Empty;
            }
            else
            {
                WordAnswer = strValidator(WordAnswer.Trim());
            }
            
            if (string.IsNullOrWhiteSpace(Description))
            {
                Description = string.Empty;
            }
            else
            {
                Description = strValidator(Description.Trim());
            }

            fileValidator(MainImageNew, modelState);
        }

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
                ListId = this.ListId,
            };
        }
    }
}
