


using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

namespace BO.Models.MenuApp.DAL.Domain
{
    public sealed class Article : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string MainImagePath { get; set; }

        

        public bool Followed { get; set; }


        public long UserId { get; set; }
        public User User { get; set; }


        public List<CustomImage> AdditionalImages { get; set; }

        public byte[] RowVersion { get; set; }


        public Article()
        {
            AdditionalImages = new List<CustomImage>();
        }

    }
}
