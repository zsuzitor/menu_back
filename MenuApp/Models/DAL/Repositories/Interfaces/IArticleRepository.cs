using BO.Models.MenuApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using MenuApp.Models.BO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuApp.Models.DAL.Repositories.Interfaces
{
    public interface IArticleRepository: IGeneralRepository<Article, long>
    {
        Task<List<Article>> GetAllUsersArticles(long userId);
        Task<List<ArticleShort>> GetAllUsersArticlesShort(long userId);
        Task<Article> GetByIdIfAccessAsync(long id, long userId);
        Task<Article> GetByIdIfAccessNoTrackAsync(long id, long userId);

        Task<bool?> ChangeFollowStatus(long id, long userId);
        Task<Article> Delete(long userId, long articleId);
        Task LoadImages(Article article);
    }
}
