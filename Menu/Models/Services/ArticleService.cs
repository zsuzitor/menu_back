

using Menu.Models.Auth.Poco;
using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Menu.Models.InputModels;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menu.Models.Services
{
    public class ArticleService: IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IImageService _imageService;


        public ArticleService(IArticleRepository articleRepository, IImageService imageService)
        {
            _articleRepository = articleRepository;
            _imageService = imageService;
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
            
            if (userInfo == null)
            {
                return null;
            }

            var article = ArticleFromInputModelNew(newArticle);
            article.UserId = userInfo.UserId;

            //TODO картинки
            article.AdditionalImages=await _imageService.GetCreatableObjects(newArticle.AdditionalImages, article.Id);
            article.MainImagePath = await _imageService.CreatePhysicalFile(newArticle.MainImageNew);

            article = await _articleRepository.Create(article);
            if (article == null)
            {
                return null;
            }


            return article;
        }


        //TODO мб оптимизировать
        public async Task<bool> Edit(ArticleInputModel newArticle, UserInfo userInfo)
        {
            
            if (userInfo == null|| newArticle.Id==null)
            {
                return false;
            }

            var oldObj=await GetByIdIfAccess((long)newArticle.Id,userInfo);

            
            if (oldObj == null)
            {
                return false;
            }

            await _articleRepository.LoadImages(oldObj);
            var changed = await FillArticleFromInputModelEdit(oldObj, newArticle);


            //TODO картинки

            //newArticle.DeletedAdditionalImages;

            //oldObj.AdditionalImages.Where(x=>newArticle.DeletedAdditionalImages.Contains(x.Id));
            var imageForDelete = new List<CustomImage>();
            var imageNewList = new List<CustomImage>();

            foreach (var oldImage in oldObj.AdditionalImages)
            {
                if (newArticle.DeletedAdditionalImages.Contains(oldImage.Id))
                {
                    imageForDelete.Add(oldImage);
                }
                else
                {
                    imageNewList.Add(oldImage);
                }
            }

            var deletedImages=await _imageService.DeleteFull(imageForDelete);
            oldObj.AdditionalImages = imageNewList;//TODO дебаг, так норм? возможно надо редачить еще массив с id

            oldObj.AdditionalImages.AddRange( await _imageService.GetCreatableObjects(newArticle.AdditionalImages,oldObj.Id));
            if (newArticle.MainImageNew != null)
            {
                oldObj.MainImagePath=await _imageService.CreatePhysicalFile(newArticle.MainImageNew);
                
            }
            


            //if (changed)//?
            {
                return await _articleRepository.Edit(oldObj);
            }

            return true;
        }

        public async Task<Article> Delete( long articleId, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            return await _articleRepository.DeleteDeep(userInfo.UserId, articleId);
           
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

        //для уже существующих объектов. , нет работы с картинками 
        private async Task<bool> FillArticleFromInputModelEdit(Article baseArticle,ArticleInputModel model)
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
