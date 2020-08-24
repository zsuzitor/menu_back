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

        public async Task<Article> ChangeFollowStatus(long id)
        {
            var article = await _db.Articles.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<Article> Delete(long userId,long articleId)
        {
            var article=await _db.Articles.FirstOrDefaultAsync(x=>x.Id==articleId&&x.UserId==userId);
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
