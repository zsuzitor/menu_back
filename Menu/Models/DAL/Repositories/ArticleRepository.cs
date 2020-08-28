using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menu.Models.DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MenuDbContext _db;
        public ArticleRepository(MenuDbContext db)
        {
            _db = db;
        }

        public async Task<List<Article>> GetAllUsersArticles(long userId)
        {
            return await _db.Articles.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Article> GetById(long id)
        {
            return await _db.Articles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Article> GetIfAccess(long id, long userId)
        {
            return await _db.Articles.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<bool?> ChangeFollowStatus(long id, long userId)
        {
            var article = await GetIfAccess(id, userId);
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
            var article = await GetIfAccess(id, userId);
            if (article == null)
            {
                return null;
            }

            article.Followed = !article.Followed;
            await _db.SaveChangesAsync();
            return article;
        }

        public async Task<Article> Create(Article newArticle)
        {
            _db.Articles.Add(newArticle);
            await _db.SaveChangesAsync();
            return newArticle;
        }


        public async Task<Article> Edit(Article newData)
        {
            _db.Articles.Attach(newData);
            await _db.SaveChangesAsync();
            return newData;
        }

        public async Task<Article> Delete(long userId, long articleId)
        {
            var article = await _db.Articles.FirstOrDefaultAsync(x => x.Id == articleId && x.UserId == userId);
            if (article == null)
            {
                return null;
            }

            _db.Remove(article);
            await _db.SaveChangesAsync();
            return article;
        }

    }
}
