


using BO.Models.DAL.Domain;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.DAL.Repositories.Interfaces;

namespace Menu.Models.Services
{
    public sealed class ImageService : IImageService
    {

        private readonly IImageRepository _imageRepository;
        private readonly IImageDataStorage _imageDataStorage;


        public ImageService(IImageRepository imgRep, IImageDataStorage imageDataStorage)
        {
            _imageRepository = imgRep;
            _imageDataStorage = imageDataStorage;
        }

        public  async Task<CustomImage> Upload(IFormFile image, Action<CustomImage> beforeCreate)
        {
            if (image == null)
            {
                return null;
            }

            var physImg = await CreateUploadFileWithOutDbRecord(image);

            var res = new CustomImage()
            {
                Path = physImg,
            };
            beforeCreate(res);

            await _imageRepository.AddAsync(res);
            
            return res;
        }

        /// <summary>
        /// создает физ файлы но не добавляет их в бд(id объектов 0)
        /// </summary>
        /// <param name="images"></param>
        /// <param name="beforeCreate"></param>
        /// <returns></returns>
        public async Task<List<CustomImage>> GetCreatableUploadObjects(List<IFormFile> images, Action<CustomImage> beforeCreate)
        {
            if (images == null || images.Count == 0)
            {
                return null;
            }

            List<CustomImage> imagesForAdd = new List<CustomImage>();
            foreach (var uploadedImg in images)
            {
                var physImg = await CreateUploadFileWithOutDbRecord(uploadedImg);

                var img = new CustomImage()
                {
                    Path = physImg,
                };
                beforeCreate(img);
                imagesForAdd.Add(img);
            }

            return imagesForAdd;
        }


        /// <summary>
        ///  возвращает готовые созданные объекты
        /// </summary>
        /// <param name="images"></param>
        /// <param name="beforeCreate"></param>
        /// <returns></returns>
        public async Task<List<CustomImage>> Upload(List<IFormFile> images, Action<CustomImage> beforeCreate)
        {
            var res = await GetCreatableUploadObjects(images, beforeCreate);
            if (res == null || res.Count == 0)
            {
                return new List<CustomImage>();
            }

            await _imageRepository.AddAsync(res);
            return res;
        }


        public async Task<string> CreateWithOutDbRecord(IFormFile image)
        {
            if (image == null)
            {
                return null;
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "-"
                + DateTime.Now.Ticks + "." + image.FileName.Split('.').Last();//TODO имя тут неправильно так передавать
            return await _imageDataStorage.CreateAsync(image.OpenReadStream(), uniqueFileName);
        }

        public async Task<string> CreateUploadFileWithOutDbRecord(IFormFile image)
        {
            if (image == null)
            {
                return null;
            }


            string uniqueFileName = Guid.NewGuid().ToString() + "-"
                + DateTime.Now.Ticks + "." + image.FileName.Split('.').Last();//TODO имя тут неправильно так передавать
            return await _imageDataStorage.CreateUploadAsync(image.OpenReadStream(), uniqueFileName);
        }



        /// <summary>
        ///  без какой либо валидации
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFileWithOutDbRecord(string path)
        {
            //TODO возможно вообще не удалять физический файлы
            //TODO ну вернется false и как это обработать?
            //todo может тут проверить что это именно картинка??
            //return await _fileService.DeletePhysicalFile(path);
            return await _imageDataStorage.DeleteAsync(path);
        }


        public async Task<CustomImage> DeleteById(long idImage)
        {
            var imgFromDb = await _imageRepository.GetAsync(idImage);
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

            var imgFromDb = await _imageRepository.GetAsync(idImages);
            return await DeleteFull(imgFromDb);
        }

        public async Task<List<CustomImage>> DeleteFull(List<CustomImage> images)
        {
            if (images == null || images?.Count == 0)
            {
                return new List<CustomImage>();
            }

            await _imageRepository.DeleteAsync(images);
            foreach (var img in images)
            {
                await DeleteFileWithOutDbRecord(img.Path);
            }

            return images;
        }


    }
}
