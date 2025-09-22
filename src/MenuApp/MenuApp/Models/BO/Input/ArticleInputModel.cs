using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MenuApp.Models.BO.Input
{
    public sealed class ArticleInputModel
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        /// <summary>
        /// we need CLEAR main image(not change, clear!)
        /// </summary>
        public bool? DeleteMainImage { get; set; }
        public IFormFile MainImageNew { get; set; }

        public List<long> DeletedAdditionalImages { get; set; }

        public List<IFormFile> AdditionalImages { get; set; }


        public ArticleInputModel()
        {
            DeleteMainImage = false;
            DeletedAdditionalImages = new List<long>();
            AdditionalImages = new List<IFormFile>();
        }
    }
}
