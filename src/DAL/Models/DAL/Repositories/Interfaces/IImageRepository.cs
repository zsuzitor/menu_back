


using BO.Models.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IImageRepository:IGeneralRepository<CustomImage,long>
    {
        Task<List<long>> GetImagesIdsByArticleId(long articleId);
    }
}
