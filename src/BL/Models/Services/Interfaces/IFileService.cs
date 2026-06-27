

using BO.Models.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IFileService
    {
        Task<CustomFile> Upload(IFormFile image, Action<CustomFile> beforeCreate);
        Task<CustomFile> Upload(IFormFile image);
        Task<List<CustomFile>> Upload(List<IFormFile> images, Action<CustomFile> beforeCreate);

        /// <summary>
        /// создает физический файл и заносит в бд
        /// </summary>
        /// <param name="images"></param>
        /// <param name="beforeCreate"></param>
        /// <returns></returns>
        Task<List<CustomFile>> GetCreatableUploadObjects(List<IFormFile> images, Action<CustomFile> beforeCreate);
        Task<CustomFile> GetCreatableUploadObjects(IFormFile images, Action<CustomFile> beforeCreate);
        Task<string> CreateWithOutDbRecord(IFormFile image);
        Task<string> CreateUploadFileWithOutDbRecord(IFormFile image);

        Task<bool> DeleteFileWithOutDbRecord(string path);
        Task<CustomFile> DeleteById(long idImage);
        Task<CustomFile> GetById(long idImage);
        Task<List<long>> GetIdsByArticleId(long idArticle);
        Task<List<CustomFile>> DeleteById(List<long> idImages);
        Task<List<CustomFile>> DeleteFull(List<CustomFile> images);


        Task DeleteNotActualFiles();

        //string GetRelativePath(string subPath);
    }
}
