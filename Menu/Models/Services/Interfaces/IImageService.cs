

using Menu.Models.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IImageService
    {
        Task<CustomImage> Upload(IFormFile image, long articleId);
        Task<List<CustomImage>> Upload(List<IFormFile> images, long articleId);
        Task<List<CustomImage>> GetCreatableObjects(List<IFormFile> images, long articleId);
        Task<string> CreatePhysicalFile(IFormFile image);

        Task<bool> DeletePhysicalFile(string path);
        Task<CustomImage> DeleteById(long idImage);
        Task<List<CustomImage>> DeleteById(List<long> idImages);
        Task<List<CustomImage>> DeleteFull(List<CustomImage> images);

    }
}
