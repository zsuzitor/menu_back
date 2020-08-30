

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu.Models.InputModels
{
    public class ArticleInputModel
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }

        public bool? DeleteMainImage { get; set; }
        public IFormFile MainImageNew { get; set; }

        public List<string> DeletedAdditionalImages { get; set; }
        public List<IFormFile> AdditionalImages { get; set; }


        public ArticleInputModel()
        {
            DeleteMainImage = false;
        }
    }
}
