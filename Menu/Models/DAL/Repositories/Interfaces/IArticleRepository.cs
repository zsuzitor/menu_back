using Menu.Models.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.DAL.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllUsersArticles(long userId);
        Task<Article> GetById(long id);
        Task<Article> ChangeFollowStatus(long id);
        Task<Article> Create(Article newArticle);
        Task<Article> Delete(long userId, long articleId);
    }
}
