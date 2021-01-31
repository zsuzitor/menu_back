


using BO.Models.DAL.Domain;
using Menu.Models.Services.Interfaces;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.DAL.Repositories.Interfaces;
using BL.Models.Services.Interfaces;
//using System.IO;

namespace Menu.Models.Services
{
    public class ImageService : IImageService
    {

        private readonly dynamic _webHostEnvironment;
        //private readonly MenuDbContext _db;
        private readonly IImageRepository _imageRepository;
        private readonly IFileService _fileService;

        //private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly MenuDbContext _db;

        //public ImageService(IWebHostEnvironment webHostEnvironment, MenuDbContext db)
        //{
        //    _webHostEnvironment = webHostEnvironment;
        //    _db = db;
        //}

        public ImageService(IImageRepository imgRep, IFileService fileService)
        {
            _imageRepository = imgRep;
            _fileService = fileService;
        }

        public async Task<CustomImage> Upload(IFormFile image, long articleId)
        {
            if (image == null)
            {
                return null;
            }

            var physImg = await CreatePhysicalUploadFile(image);

            var res = new CustomImage()
            {
                ArticleId = articleId,
                Path = physImg,
            };

            await _imageRepository.Add(res);
            
            return res;
        }

        /// <summary>
        /// создает физ файлы но не добавляет их в бд(id объектов 0)
        /// </summary>
        /// <param name="images"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<CustomImage>> GetCreatableUploadObjects(List<IFormFile> images, long articleId)
        {
            if (images == null || images.Count == 0)
            {
                return null;
            }

            List<CustomImage> imagesForAdd = new List<CustomImage>();
            foreach (var uploadedImg in images)
            {
                var physImg = await CreatePhysicalUploadFile(uploadedImg);

                var img = new CustomImage()
                {
                    ArticleId = articleId,
                    Path = physImg,
                };
                imagesForAdd.Add(img);
            }

            //_db.Images.AddRange(imagesForAdd);
            //await _db.SaveChangesAsync();
            return imagesForAdd;
        }


        /// <summary>
        /// возвращает готовые созданные объекты
        /// </summary>
        /// <param name="images"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<CustomImage>> Upload(List<IFormFile> images, long articleId)
        {
            var res = await GetCreatableUploadObjects(images, articleId);
            if (res == null || res.Count == 0)
            {
                return new List<CustomImage>();
            }

            await _imageRepository.Add(res);
            return res;
        }


        public async Task<string> CreatePhysicalFile(IFormFile image)
        {
            return await CreatePhysicalFile(image, string.Empty);
        }

        public async Task<string> CreatePhysicalUploadFile(IFormFile image)
        {
            return await CreatePhysicalFile(image, "uploads");
        }



        /// <summary>
        ///  без какой либо валидации
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<bool> DeletePhysicalFile(string path)
        {
            //TODO возможно вообще не удалять физический файлы
            //TODO ну вернется false и как это обработать?
            //todo может тут проверить что это именно картинка??
            return await _fileService.DeletePhysicalFile(path);
        }


        public async Task<CustomImage> DeleteById(long idImage)
        {
            var imgFromDb = await _imageRepository.Get(idImage);
            if (imgFromDb == null)
            {
                return null;
            }
            return (await DeleteFull(new List<CustomImage>() { imgFromDb })).FirstOrDefault();
        }

        public async Task<List<long>> GetIdsByArticleId(long idArticle)
        {
            return await _imageRepository.GetImagesIdsByArticleId(idArticle);
        }

        //до вызова надо проверить можно ли получить доступ
        public async Task<List<CustomImage>> DeleteById(List<long> idImages)
        {
            if (idImages == null || idImages.Count == 0)
            {
                return new List<CustomImage>();
            }

            var imgFromDb = await _imageRepository.Get(idImages);
            return await DeleteFull(imgFromDb);
        }

        public async Task<List<CustomImage>> DeleteFull(List<CustomImage> images)
        {
            if (images == null || images?.Count == 0)
            {
                return new List<CustomImage>();
            }

            await _imageRepository.Delete(images);
            foreach (var img in images)
            {
                await DeletePhysicalFile(img.Path);
            }

            return images;
        }



        private async Task<string> CreatePhysicalFile(IFormFile image, string subPath)
        {
            if (image == null)
            {
                return null;
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string resPath = _fileService.PathCombine(subPath, uniqueFileName);
            //string uploadsFolder = Path.Combine("images", resPath);

            string filePath = _fileService.PathCombine(_webHostEnvironment.WebRootPath, "images", resPath);
            //using (MemoryStream memStream = new MemoryStream((int)image.Length))//todo ing??
            //{
            //    image.OpenReadStream();
            //}
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))//TODO будет ли тут исключение если файл существует?
            //{
            //    await image.CopyToAsync(fileStream);
            //    image.len
            //}

            await _fileService.Create(image.OpenReadStream(), filePath);

            return resPath;
        }

        /// <summary>
        /// везде путь хранится от серва а от какой то папка картинок, например wwwroot\images\{savedPath}, в таком случае вернет  images\{savedPath}
        /// </summary>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public string GetRelativePath(string subPath)
        {
            return _fileService.PathCombine("images", subPath);
        }

    }
}
