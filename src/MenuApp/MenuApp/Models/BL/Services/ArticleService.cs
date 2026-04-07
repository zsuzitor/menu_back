


using BO.Models.Auth;
using BO.Models.MenuApp.DAL.Domain;
using Common.Models.Error;
using Common.Models.Exceptions;
using DAL.Models.DAL;
using Hangfire.Common;
using Menu.Models.Services.Interfaces;
using MenuApp.Models.BO;
using MenuApp.Models.BO.Input;
using MenuApp.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuApp.Models.BL.Services
{
    public sealed class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IImageService _imageService;
        private readonly IDBHelper _dbHelper;
        private readonly MenuDbContext _db;


        public ArticleService(IArticleRepository articleRepository, IImageService imageService
            , IDBHelper dbHelper, MenuDbContext db)
        {
            _articleRepository = articleRepository;
            _imageService = imageService;
            _dbHelper = dbHelper;
            _db = db;
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
                await _dbHelper.ActionInTransaction(_db, async () =>
                {
                    var image = await _imageService.Upload(newArticle.MainImageNew);
                    article.ImageId = image?.Id;
                    article = await _articleRepository.AddAsync(article);
                    article.AdditionalImages = await _imageService.Upload(newArticle.AdditionalImages, (img) => img.ArticleId = article.Id);
                    article.Image = image;
                });
                
                return article;
            }
            catch
            {

                //if (article.AdditionalImages?.Count > 0)
                //{
                //    //TODO картинки надо попытаться удалить
                //    await _imageService.DeleteFull(article.AdditionalImages);
                //}
                throw;
            }
        }



        public async Task<Article> Update(ArticleInputModel newArticle, UserInfo userInfo)
        {

            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            if (newArticle?.Id == null)
            {
                throw new SomeCustomException("id_is_required");//TODO
            }


            Article oldObj = null;
            await _dbHelper.ActionInTransaction(_db, async () =>
            {
                oldObj = await GetByIdIfAccess((long)newArticle.Id, userInfo);

                if (oldObj == null)
                {
                    throw new SomeCustomException(ErrorConsts.NotFound);
                }

                var changed = FillArticleFromInputModelEdit(oldObj, newArticle);

                if (newArticle.MainImageNew != null)
                {
                    if (oldObj.ImageId.HasValue)
                        await _imageService.DeleteById(oldObj.ImageId.Value);
                    oldObj.Image = await _imageService.Upload(newArticle.MainImageNew);
                    oldObj.ImageId = oldObj.Image.Id;
                    changed = true;
                }
                else if (newArticle.DeleteMainImage ?? false && oldObj.ImageId.HasValue)
                {
                    await _imageService.DeleteById(oldObj.ImageId.Value);
                    oldObj.ImageId = null;
                    oldObj.Image = null;
                    changed = true;

                }

                if (changed)//?
                {
                    await _articleRepository.UpdateAsync(oldObj);
                }

                var newImages = await _imageService.Upload(newArticle.AdditionalImages, (img) => img.ArticleId = oldObj.Id);

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
            });
            
            

            return oldObj;
        }

        public async Task<Article> Delete(long articleId, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            var deletedArticle = await _articleRepository.Delete(userInfo.UserId, articleId);
            if (deletedArticle != null)
            {
                await _imageService.DeleteFull(deletedArticle.AdditionalImages);
                if(deletedArticle.ImageId.HasValue)
                await _imageService.DeleteById(deletedArticle.ImageId.Value);
            }

            return deletedArticle;

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
            return await _articleRepository.GetNoTrackAsync(id);
        }

        public async Task<Article> GetByIdIfAccess(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _articleRepository.GetByIdIfAccessAsync(id, userInfo.UserId);
        }

        public async Task<Article> GetFullByIdIfAccess(long id, UserInfo userInfo)
        {
            //todo тут хорошо бы noTrack
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            var article = await _articleRepository.GetByIdIfAccessAsync(id, userInfo.UserId);
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
        private bool FillArticleFromInputModelEdit(Article baseObj, ArticleInputModel newObj)
        {
            bool changed = false;
            if (baseObj.Body != newObj.Body)
            {
                baseObj.Body = newObj.Body;
                changed = true;
            }

            if (baseObj.Title != newObj.Title)
            {
                baseObj.Title = newObj.Title;
                changed = true;
            }

            return changed;
        }

    }
}
