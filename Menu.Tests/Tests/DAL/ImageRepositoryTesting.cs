

using BO.Models.DAL.Domain;
using BO.Models.MenuApp.DAL.Domain;
using DAL.Models.DAL.Repositories;
using Menu.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Menu.Tests.Tests.Dal
{
    public class ImageRepositoryTesting : TestBase
    {
        [Theory]
        [InlineData(-1)]
        public  void test_test(int num)
        {
            Assert.Equal(num, -1);

        }

        [Fact]
        public async Task CreateNewImageWithArticle_Created()
        {

            using var db = GetDbContext();

            new DefaultData().InitDbDefault(db);
            ImageRepository imgRepo = new ImageRepository(db);
            var newArticle = new Article()
            {
                Body = "body1",
                Title = "title1",
            };

            db.Articles.Add(newArticle);
            db.SaveChanges();

            var imgForAdd = new CustomImage()
            {
                Path = "10",
                ArticleId = newArticle.Id,
            };

            await imgRepo.AddAsync(imgForAdd);
            var addedImg = db.Images.FirstOrDefault(x => x.Id == imgForAdd.Id);
            Assert.NotNull(addedImg);
            Assert.Equal(imgForAdd.Path, addedImg.Path);
            Assert.Equal(imgForAdd.ArticleId, addedImg.ArticleId);

        }

        [Fact]
        public async Task CreateNewImages_Created()
        {

            using var db = GetDbContext();

            new DefaultData().InitDbDefault(db);
            ImageRepository imgRepo = new ImageRepository(db);

            var articlesForAdd = new List<Article>()
            {
                new Article()
                {
                    Body = "body1",
                    Title = "title1",
                },
                new Article()
                {
                    Body = "body2",
                    Title = "title2",
                },
            };


            db.Articles.AddRange(articlesForAdd);
            db.SaveChanges();

            var imagesForAdd = new List<CustomImage>()
            {
                new CustomImage()
                {
                    Path = "10",
                    ArticleId = articlesForAdd[0].Id,
                },
                new CustomImage()
                {
                    Path = "11",
                    ArticleId = articlesForAdd[1].Id,
                },
            };

            await imgRepo.AddAsync(imagesForAdd);
            var addedImg1 = db.Images.FirstOrDefault(x => x.Id == imagesForAdd[0].Id);
            var addedImg2 = db.Images.FirstOrDefault(x => x.Id == imagesForAdd[1].Id);
            Assert.NotNull(addedImg1);
            Assert.Equal(addedImg1.Path, imagesForAdd[0].Path);
            Assert.Equal(addedImg1.ArticleId, imagesForAdd[0].ArticleId);

            Assert.NotNull(addedImg2);
            Assert.Equal(addedImg2.Path, imagesForAdd[1].Path);
            Assert.Equal(addedImg2.ArticleId, imagesForAdd[1].ArticleId);

        }


        [Fact]
        public async Task DeleteImage_Deleted()
        {

            using var db = GetDbContext();

            new DefaultData().InitDbDefault(db);
            ImageRepository imgRepo = new ImageRepository(db);

            var delImg = db.Images.OrderBy(x => x.Id).Skip(1).First();
            var deleted = (await imgRepo.DeleteAsync(new List<CustomImage>() { delImg })).ToList();

            Assert.NotNull(deleted);
            Assert.Single(deleted);
            Assert.Equal(delImg.Path, deleted[0].Path);
            Assert.Equal(delImg.Id, deleted[0].Id);
            Assert.Equal(delImg.ArticleId, deleted[0].ArticleId);

        }
    }
}
