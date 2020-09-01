

using Menu.Models.DAL;
using Menu.Models.DAL.Domain;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Menu.Models.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MenuDbContext _db;

        public ImageService(IWebHostEnvironment webHostEnvironment, MenuDbContext db)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public async Task<CustomImage> Upload(IFormFile image, long articleId)
        {
            if (image == null)
            {
                return null;
            }

            var physImg = await CreatePhysicalFile(image);

            var res = new CustomImage()
            {
                ArticleId = articleId,
                Path = physImg,
            };
            _db.Images.Add(res);
            await _db.SaveChangesAsync();
            return res;
        }

       
        public async Task<List<CustomImage>> GetCreatableObjects(List<IFormFile> images, long articleId)
        {
            if (images == null|| images.Count==0)
            {
                return null;
            }

            List<CustomImage> imagesForAdd = new List<CustomImage>();
            foreach (var uploadedImg in images)
            {
                var physImg = await CreatePhysicalFile(uploadedImg);

                var img = new CustomImage()
                {
                    ArticleId = articleId,
                    Path = physImg,
                };
                imagesForAdd.Add(img);
            }

            _db.Images.AddRange(imagesForAdd);
            await _db.SaveChangesAsync();
            return imagesForAdd;
        }

        public async Task<List<CustomImage>> Upload(List<IFormFile> images, long articleId)
        {
            var res=await GetCreatableObjects(images, articleId);
            if (res == null)
            {
                return null;
            }

            _db.Images.AddRange(res);
            await _db.SaveChangesAsync();
            return res;
        }


        public async Task<string> CreatePhysicalFile(IFormFile image)
        {
            if (image == null)
            {
                return null;
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))//TODO будет ли тут исключение если файл существует?
            {
                await image.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }


        public async Task<bool> DeletePhysicalFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);//TODO если файл кто то читает будет ли ошибка?
                }
            }
            catch
            {
                return false;
            }

            return true;
        }


        public async Task<CustomImage> DeleteById(long idImage)
        {
            var imgFromDb = await _db.Images.FirstOrDefaultAsync(x => x.Id==idImage);
            if(imgFromDb==null)
            {
                return null;
            }
            return (await DeleteFull(new List<CustomImage>() { imgFromDb })).FirstOrDefault();
        }

        public async Task<List<long>> GetIdsByArticleId(long idArticle)
        {
            return await _db.Images.Where(x=>x.ArticleId==idArticle).Select(x=>x.Id).ToListAsync();
        }

        //до вызова надо проверить можно ли получить доступ
        public async Task<List<CustomImage>> DeleteById(List<long> idImages)
        {
            if (idImages == null || idImages?.Count == 0)
            {
                return new List<CustomImage>();
            }

            var imgFromDb = await _db.Images.Where(x => idImages.Contains(x.Id)).ToListAsync();
            return await DeleteFull(imgFromDb);
        }

        public async Task<List<CustomImage>> DeleteFull(List<CustomImage> images)
        {
            if (images == null || images?.Count == 0)
            {
                return new List<CustomImage>();
            }

            _db.Images.AttachRange(images);

            _db.RemoveRange(images);
            await _db.SaveChangesAsync();
            foreach (var img in images)
            {
                await DeletePhysicalFile(img.Path);
            }

            return images;
        }


    }
}
