

using Menu.Models.Auth.Poco;
using Menu.Models.DAL.Domain;
using Menu.Models.InputModels;
using Menu.Models.Poco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IArticleService
    {
        Task<List<Article>> GetAllUsersArticles(UserInfo userInfo);
        Task<List<ArticleShort>> GetAllUsersArticlesShort(UserInfo userInfo);
        
        Task<Article> GetById(long id);
        Task<Article> GetByIdIfAccess(long id, UserInfo userInfo);
        Task<Article> GetFullByIdIfAccess(long id, UserInfo userInfo);


        /// <summary>
        /// return true если картонка зафоловлена после изменений
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<bool> ChangeFollowStatus(long id, UserInfo userInfo);
        Task<Article> Create(ArticleInputModel newArticle, UserInfo userInfo);
        Task<Article> Edit(ArticleInputModel newArticle, UserInfo userInfo);
        
        Task<Article> Delete(long articleId, UserInfo userInfo);
    }
}
