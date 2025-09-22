using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using WordsCardsApp.BO.Input;

namespace Menu.Models.WordsCardsApp.Requests
{
    public sealed class WordCardListInputModelApi
    {
        [BindProperty(Name = "id", SupportsGet = false)]
        public long? Id { get; set; }

        [BindProperty(Name = "title", SupportsGet = false)]
        [Required]
        public string Title { get; set; }



        public void Validate(Func<string, string> strValidator, ModelStateDictionary modelState)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = string.Empty;
            }
            else
            {
                Title = strValidator(Title.Trim());
            }

        }

        public WordCardListInputModel GetModel()
        {
            return new WordCardListInputModel()
            {
                Id = Id,
                Title = Title,
            };
        }
    }
}
