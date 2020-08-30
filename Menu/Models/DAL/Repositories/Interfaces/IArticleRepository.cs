using Menu.Models.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.DAL.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllUsersArticles(long userId);
        Task<Article> GetById(long id);
        Task<Article> GetByIdIfAccess(long id,long userId);
        
        Task<bool?> ChangeFollowStatus(long id, long userId);
        Task<Article> Create(Article newArticle);
        Task<bool> Edit(Article newData);
        Task<Article> DeleteDeep(long userId, long articleId);
        Task LoadImages(Article article);
    }
}
