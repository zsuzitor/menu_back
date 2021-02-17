

using jwtLib.JWTAuth.Interfaces;
using System.Collections.Generic;
using BO.Models.MenuApp.DAL.Domain;
using BO.Models.WordsCardsApp.DAL.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO.Models.DAL.Domain
{
    public class User: IJWTUser, IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
       
        public string ImagePath { get; set; }
        public string PasswordHash { get; set; }
        public string RefreshTokenHash { get; set; }

        public List<Article> Articles { get; set; }
        public List<WordCard> WordsCards { get; set; }
        public List<WordsList> WordsLists { get; set; }
        

        public User()
        {
            Articles = new List<Article>();
            WordsCards = new List<WordCard>();
            WordsLists = new List<WordsList>();
        }
    }
}
