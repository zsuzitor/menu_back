

using Menu.Models.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IArticleService
    {
        Task<List<Article>> GetAllUsersArticles(long userId);
        Task<Article> GetById(long id);
        Task<Article> GetByIdIfAccess(long id, long userId);
        Task<Article> GetByIdIfAccess(long id, string userId);

        Task<bool?> ChangeFollowStatus(long id, long userId);
        Task<bool?> ChangeFollowStatus(long id, string userId);
        Task<Article> Create(Article newArticle);
        Task<Article> Delete(long userId, long articleId);
    }
}
