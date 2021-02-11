using BO.Models.MenuApp.DAL.Domain;
using MenuApp.Models.BO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuApp.Models.DAL.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllUsersArticles(long userId);
        Task<List<ArticleShort>> GetAllUsersArticlesShort(long userId);
        Task<Article> GetById(long id);
        Task<Article> GetByIdIfAccess(long id, long userId);

        Task<bool?> ChangeFollowStatus(long id, long userId);
        Task<Article> Create(Article newArticle);
        Task Edit(Article newData);
        Task<Article> Delete(long userId, long articleId);
        Task LoadImages(Article article);
    }
}
