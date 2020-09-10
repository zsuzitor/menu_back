using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Menu.Models.Poco;
using Menu.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menu.Models.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MenuDbContext _db;
        private readonly IImageService _imageService;


        public ArticleRepository(MenuDbContext db, IImageService imageService)
        {
            _db = db;
            _imageService = imageService;
        }

        public async Task<List<Article>> GetAllUsersArticles(long userId)
        {
            return await _db.Articles.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<ArticleShort>> GetAllUsersArticlesShort(long userId)
        {
            return await _db.Articles.Where(x => x.UserId == userId).Select(x => new ArticleShort(x)).ToListAsync();
        }

        public async Task<Article> GetById(long id)
        {
            return await _db.Articles.FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<bool?> ChangeFollowStatus(long id, long userId)
        {
            var article = await GetByIdIfAccess(id, userId);
            if (article == null)
            {
                return null;
            }

            article.Followed = !article.Followed;
            await _db.SaveChangesAsync();
            return article.Followed;
        }

        public async Task<Article> GetByIdIfAccess(long id, long userId)
        {
            return await _db.Articles.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<Article> Create(Article newArticle)
        {
            _db.Articles.Add(newArticle);
            await _db.SaveChangesAsync();
            return newArticle;
        }


        public async Task LoadImages(Article article)
        {
            _db.Articles.Attach(article);
            await _db.Entry(article).Collection(x => x.AdditionalImages).LoadAsync();
        }

        public async Task Edit(Article newData)
        {
            _db.Articles.Attach(newData);

            //await _db.Entry(newData).Collection(x => x.AdditionalImages).LoadAsync();

            await _db.SaveChangesAsync();
            //return true;
        }

        public async Task<Article> DeleteDeep(long userId, long articleId)
        {
            //TODO в транзакцию? но физ файлы поудаляются и что делать? сообщение о том что удаление прервано и последствия неизвестны?
            var article = await _db.Articles.FirstOrDefaultAsync(x => x.Id == articleId && x.UserId == userId);
            if (article == null)
            {
                return null;
            }

            await _db.Entry(article).Collection(x => x.AdditionalImages).LoadAsync();
            await _imageService.DeleteFull(article.AdditionalImages);

            _db.Remove(article);
            await _db.SaveChangesAsync();
            return article;
        }

    }
}
