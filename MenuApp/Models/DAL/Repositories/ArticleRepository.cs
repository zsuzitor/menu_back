using BO.Models.MenuApp.DAL.Domain;
using MenuApp.Models.DAL.Repositories.Interfaces;
//using Menu.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.DAL;
using MenuApp.Models.BO;
using DAL.Models.DAL.Repositories;

namespace MenuApp.Models.DAL.Repositories
{
    public class ArticleRepository : GeneralRepository<Article, long>, IArticleRepository
    {


        public ArticleRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<List<Article>> GetAllUsersArticles(long userId)
        {
            return await _db.Articles.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<ArticleShort>> GetAllUsersArticlesShort(long userId)
        {
            return await _db.Articles.AsNoTracking().Where(x => x.UserId == userId).Select(x => new ArticleShort(x)).ToListAsync();
        }

        public async Task<bool?> ChangeFollowStatus(long id, long userId)
        {
            var article = await GetByIdIfAccessAsync(id, userId);
            if (article == null)
            {
                return null;
            }

            article.Followed = !article.Followed;
            await _db.SaveChangesAsync();
            return article.Followed;
        }

        public async Task<Article> GetByIdIfAccessAsync(long id, long userId)
        {
            return await _db.Articles.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<Article> GetByIdIfAccessNoTrackAsync(long id, long userId)
        {
            return await _db.Articles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }


        public async Task LoadImages(Article article)
        {
            _db.Articles.Attach(article);
            await _db.Entry(article).Collection(x => x.AdditionalImages).LoadAsync();
        }


        public async Task<Article> Delete(long userId, long articleId)
        {
            //TODO в транзакцию? но физ файлы поудаляются и что делать? сообщение о том что удаление прервано и последствия неизвестны?
            var article = await _db.Articles.FirstOrDefaultAsync(x => x.Id == articleId && x.UserId == userId);
            if (article == null)
            {
                return null;
            }

            await _db.Entry(article).Collection(x => x.AdditionalImages).LoadAsync();

            _db.Remove(article);
            await _db.SaveChangesAsync();
            return article;
        }

    }
}
