

using Menu.Models.Auth.Poco;
using Menu.Models.DAL.Domain;
using Menu.Models.InputModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IArticleService
    {
        Task<List<Article>> GetAllUsersArticles(UserInfo userInfo);
        Task<Article> GetById(long id);
        Task<Article> GetByIdIfAccess(long id, UserInfo userInfo);
        

        Task<bool?> ChangeFollowStatus(long id, UserInfo userInfo);
        Task<Article> Create(ArticleInputModel newArticle, UserInfo userInfo);
        Task<bool> Edit(ArticleInputModel newArticle, UserInfo userInfo);
        
        Task<Article> Delete(long articleId, UserInfo userInfo);
    }
}
