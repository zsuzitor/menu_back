

using Menu.Models.Auth.Poco;
using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Menu.Models.Error;
using Menu.Models.Error.Interfaces;
using Menu.Models.Error.services.Interfaces;
using Menu.Models.Exceptions;
using Menu.Models.InputModels;
using Menu.Models.Poco;
using Menu.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IImageService _imageService;
        private readonly IErrorService _errorService;
        private readonly IErrorContainer _errorContainer;


        public ArticleService(IArticleRepository articleRepository, IImageService imageService,
            IErrorService errorService, IErrorContainer errorContainer)
        {
            _articleRepository = articleRepository;
            _imageService = imageService;
        }

        //
        public async Task<bool> ChangeFollowStatus(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            var changed = await _articleRepository.ChangeFollowStatus(id, userInfo.UserId);
            if (changed == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return (bool)changed;
        }


        public async Task<Article> Create(ArticleInputModel newArticle, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            var article = ArticleFromInputModelNew(newArticle);
            article.UserId = userInfo.UserId;
            try
            {
                article.MainImagePath = await _imageService.CreatePhysicalUploadFile(newArticle.MainImageNew);

                article = await _articleRepository.Create(article);


                article.AdditionalImages = await _imageService.Upload(newArticle.AdditionalImages, article.Id);

                return article;
            }
            catch
            {

                if (article.AdditionalImages?.Count > 0)
                {
                    //TODO картинки надо попытаться удалить
                    await _imageService.DeleteFull(article.AdditionalImages);
                }
                throw;
            }
        }



        public async Task<Article> Edit(ArticleInputModel newArticle, UserInfo userInfo)
        {

            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            if (newArticle?.Id == null)
            {
                throw new SomeCustomException("id_is_required");//TODO
            }

            var oldObj = await GetByIdIfAccess((long)newArticle.Id, userInfo);


            if (oldObj == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            
            var changed = FillArticleFromInputModelEdit(oldObj, newArticle);
            


            if (newArticle.MainImageNew != null)
            {
                await _imageService.DeletePhysicalFile(oldObj.MainImagePath);
                oldObj.MainImagePath = await _imageService.CreatePhysicalUploadFile(newArticle.MainImageNew);
                changed = true;
            }
            else if (newArticle.DeleteMainImage ?? false && !string.IsNullOrWhiteSpace(oldObj.MainImagePath))
            {
                await _imageService.DeletePhysicalFile(oldObj.MainImagePath);
                oldObj.MainImagePath = null;
                changed = true;

            }

            if (changed)//?
            {
                await _articleRepository.Edit(oldObj);
            }

            var newImages = await _imageService.Upload(newArticle.AdditionalImages, oldObj.Id);

            //удаляем в конце тк самая неважная операция и самая ломающая
            
            //var imageNewList = new List<CustomImage>();

            if (newArticle.DeletedAdditionalImages.Count > 0)
            {
                var imageForDelete = new List<long>();
                var oldImagesId = await _imageService.GetIdsByArticleId(oldObj.Id);
                foreach (var oldImage in oldImagesId)
                {
                    if (newArticle.DeletedAdditionalImages.Contains(oldImage))
                    {
                        imageForDelete.Add(oldImage);
                    }
                }

                var deletedImages = await _imageService.DeleteById(imageForDelete);
            }
            

            return oldObj;
        }

        public async Task<Article> Delete(long articleId, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _articleRepository.DeleteDeep(userInfo.UserId, articleId);

        }

        public async Task<List<Article>> GetAllUsersArticles(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _articleRepository.GetAllUsersArticles(userInfo.UserId);
        }

        public async Task<List<ArticleShort>> GetAllUsersArticlesShort(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _articleRepository.GetAllUsersArticlesShort(userInfo.UserId);
        }

        public async Task<Article> GetById(long id)
        {
            return await _articleRepository.GetById(id);
        }

        public async Task<Article> GetByIdIfAccess(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _articleRepository.GetByIdIfAccess(id, userInfo.UserId);
        }

        public async Task<Article> GetFullByIdIfAccess(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            var article = await _articleRepository.GetByIdIfAccess(id, userInfo.UserId);
            if (article == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }
            await _articleRepository.LoadImages(article);
            return article;
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
        private bool FillArticleFromInputModelEdit(Article baseArticle, ArticleInputModel model)
        {
            bool changed = false;
            if (baseArticle.Body != model.Body)
            {
                baseArticle.Body = model.Body;
                changed = true;
            }

            if (baseArticle.Title != model.Title)
            {
                baseArticle.Title = model.Title;
                changed = true;
            }

            

            return changed;
        }

    }
}
