

using MenuApp.Models.BO.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu.Models.InputModels.MenuApp
{
    public class ArticleInputModelApi
    {
        [BindProperty(Name = "id", SupportsGet = false)]
        public long? Id { get; set; }
        [Required]
        [BindProperty(Name = "title", SupportsGet = false)]
        public string Title { get; set; }
        [Required]
        [BindProperty(Name = "body", SupportsGet = false)]
        public string Body { get; set; }

        /// <summary>
        /// we need CLEAR main image(not change, clear!)
        /// </summary>
        [BindProperty(Name = "delete_main_image", SupportsGet = false)]
        public bool? DeleteMainImage { get; set; }
        [BindProperty(Name = "main_image_new", SupportsGet = false)]
        public IFormFile MainImageNew { get; set; }

        [BindProperty(Name = "deleted_additional_images", SupportsGet = false)]
        public List<long> DeletedAdditionalImages { get; set; }

        [BindProperty(Name = "additional_images", SupportsGet = false)]
        public List<IFormFile> AdditionalImages { get; set; }


        public ArticleInputModelApi()
        {
            DeleteMainImage = false;
            DeletedAdditionalImages = new List<long>();
            AdditionalImages = new List<IFormFile>();
        }

        public ArticleInputModel GetModel()
        {
            return new ArticleInputModel()
            {
                Id = this.Id,
                Title = this.Title,
                Body = this.Body,
                DeleteMainImage = this.DeleteMainImage,
                MainImageNew = this.MainImageNew,
                DeletedAdditionalImages = this.DeletedAdditionalImages,
                AdditionalImages = this.AdditionalImages,
            };
        }
    }
}
