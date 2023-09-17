

using BO.Models.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IImageService
    {
        Task<CustomImage> Upload(IFormFile image, Action<CustomImage> beforeCreate);
        Task<List<CustomImage>> Upload(List<IFormFile> images, Action<CustomImage> beforeCreate);

        /// <summary>
        /// создает физический файл и заносит в бд
        /// </summary>
        /// <param name="images"></param>
        /// <param name="beforeCreate"></param>
        /// <returns></returns>
        Task<List<CustomImage>> GetCreatableUploadObjects(List<IFormFile> images, Action<CustomImage> beforeCreate);
        Task<string> CreateWithOutDbRecord(IFormFile image);
        Task<string> CreateUploadFileWithOutDbRecord(IFormFile image);

        Task<bool> DeleteFileWithOutDbRecord(string path);
        Task<CustomImage> DeleteById(long idImage);
        Task<List<long>> GetIdsByArticleId(long idArticle);
        Task<List<CustomImage>> DeleteById(List<long> idImages);
        Task<List<CustomImage>> DeleteFull(List<CustomImage> images);

        //string GetRelativePath(string subPath);
    }
}
