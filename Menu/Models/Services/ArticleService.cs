

using Menu.Models.Auth.Poco;
using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Menu.Models.InputModels;
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

        public  async Task<bool?> ChangeFollowStatus(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            return await _articleRepository.ChangeFollowStatus(id, userInfo.UserId);
        }


        public async Task<Article> Create(ArticleInputModel newArticle, UserInfo userInfo)
        {
            //TODO картинки
            if (userInfo == null)
            {
                return null;
            }

            var article = ArticleFromInputModelNew(newArticle);
            article.UserId = userInfo.UserId;
            return await _articleRepository.Create(article);
        }


        public async Task<Article> Edit(ArticleInputModel newArticle, UserInfo userInfo)
        {
            //TODO картинки
            var oldObj=await GetByIdIfAccess(newArticle.Id,userInfo);
            if (oldObj == null)
            {
                return null;
            }

            var changed=FillArticleFromInputModelEdit(oldObj,newArticle);
            if (changed)
            {
                return await _articleRepository.Edit(oldObj);
            }

            return oldObj;
        }

        public async Task<Article> Delete( long articleId, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            return await _articleRepository.Delete(userInfo.UserId, articleId);
        }

        public async Task<List<Article>> GetAllUsersArticles(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            return await _articleRepository.GetAllUsersArticles(userInfo.UserId);
        }

        public async Task<Article> GetById(long id)
        {
            return await _articleRepository.GetById(id);
        }

        public async Task<Article> GetByIdIfAccess(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            return await _articleRepository.GetByIdIfAccess(id, userInfo.UserId);
        }

       


        private Article ArticleFromInputModelNew(ArticleInputModel model)
        {
            
            return new Article
            {
                Body = model.Body,
                Title = model.Title
            };
        }

        private bool FillArticleFromInputModelEdit(Article baseArticle,ArticleInputModel model)
        {
            bool changed = false;
            if(baseArticle.Body!= model.Body)
            {
                baseArticle.Body = model.Body;
                changed = true;
            }

            if (baseArticle.Title != model.Title)
            {
                baseArticle.Title = model.Title;
                changed = true;
            }
            
            if (model.DeleteMainImage??false)
            {
                if (baseArticle.MainImagePath != null)
                {
                    changed = true;
                }
                baseArticle.MainImagePath = null;
            }


            return changed;
        }

    }
}
