

using BO.Models.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IImageService
    {
        Task<CustomImage> Upload(IFormFile image, long articleId);
        Task<List<CustomImage>> Upload(List<IFormFile> images, long articleId);

        /// <summary>
        /// создает физический файл и заносит в бд
        /// </summary>
        /// <param name="images"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<CustomImage>> GetCreatableUploadObjects(List<IFormFile> images, long articleId);
        Task<string> CreatePhysicalFile(IFormFile image);
        Task<string> CreatePhysicalUploadFile(IFormFile image);

        Task<bool> DeletePhysicalFile(string path);
        Task<CustomImage> DeleteById(long idImage);
        Task<List<long>> GetIdsByArticleId(long idArticle);
        Task<List<CustomImage>> DeleteById(List<long> idImages);
        Task<List<CustomImage>> DeleteFull(List<CustomImage> images);

        string GetRelativePath(string subPath);
    }
}
