

using BO.Models.DAL.Domain;
using BO.Models.MenuApp.DAL.Domain;
using DAL.Models.DAL;
using System.Collections.Generic;

namespace Menu.Tests.Models
{
    public class DefaultData
    {
        public void InitDbDefault(MenuDbContext db)
        {
            var usersForAdd = new List<User>()
            {
                new User()
                {
                    Email = "test_Email_1@mail.ru",
                    Login = "test_Login_1@mail.ru",
                    Name = "test_Email_1",
                    PasswordHash = "pas_hash_1",

                },
                new User()
                {
                    Email = "test_Email_2@mail.ru",
                    Login = "test_Login_2@mail.ru",
                    Name = "test_Email_2",
                    PasswordHash = "pas_hash_2",

                },
            };

            db.Users.AddRange(usersForAdd);
            db.SaveChanges();

            var articlesForAdd = new List<Article>()
            {
                new Article()
                {
                    Title = "article_title_1",
                    Body = "article_body_1",
                    Followed = false,
                    UserId = usersForAdd[0].Id,
                },
                new Article()
                {
                    Title = "article_title_2",
                    Body = "article_body_2",
                    Followed = false,
                    UserId = usersForAdd[1].Id,
                },
            };

            db.Articles.AddRange(articlesForAdd);
            db.SaveChanges();
            var imagesForAdd = new List<CustomImage>()
            {
                new CustomImage()
                {
                    Path = "test_path_1",
                    ArticleId = articlesForAdd[0].Id,
                },
                new CustomImage()
                {
                    Path = "test_path_2",
                    ArticleId = articlesForAdd[0].Id,
                },
            };
            db.Images.AddRange(imagesForAdd);
            db.SaveChanges();


            //db.SaveChanges();
        }

    }
}
