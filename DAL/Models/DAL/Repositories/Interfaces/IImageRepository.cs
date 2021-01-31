


using BO.Models.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<CustomImage> Add(CustomImage newImage);
        Task<List<CustomImage>> Add(List<CustomImage> newImages);

        Task<CustomImage> Get(long id);
        Task<List<CustomImage>> Get(List<long> ids);
        Task<List<CustomImage>> Delete(List<CustomImage> images);
        Task<List<long>> GetImagesIdsByArticleId(long articleId);
    }
}
