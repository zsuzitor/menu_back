

using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Menu.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services
{
    public class ArticleService: IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public  async Task<bool?> ChangeFollowStatus(long id,long userId)
        {
            return await _articleRepository.ChangeFollowStatus(id, userId);
        }

        public async Task<bool?> ChangeFollowStatus(long id, string userId)
        {
            if (!long.TryParse(userId, out long userIdLong))
            {
                return null;
            }
            return await _articleRepository.ChangeFollowStatus(id, userIdLong);
        }

        public async Task<Article> Create(Article newArticle)
        {
            return await _articleRepository.Create(newArticle);
        }

        public async Task<Article> Delete(long userId, long articleId)
        {
            return await _articleRepository.Delete(userId, articleId);
        }

        public async Task<List<Article>> GetAllUsersArticles(long userId)
        {
            return await _articleRepository.GetAllUsersArticles( userId);
        }

        public async Task<Article> GetById(long id)
        {
            return await _articleRepository.GetById(id);
        }

        public async Task<Article> GetByIdIfAccess(long id, long userId)
        {
            return await _articleRepository.GetByIdIfAccess(id, userId);
        }

        public async Task<Article> GetByIdIfAccess(long id, string userId)
        {
            if (!long.TryParse(userId, out long userIdLong))
            {
                return null;
            }
                return await _articleRepository.GetByIdIfAccess(id, userIdLong);
        }
    }
}
