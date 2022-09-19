

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IStoryRepository : IGeneralRepository<PlaningStoryDal, long>
    {
        Task<List<PlaningStoryDal>> GetActualForRoomAsync(long roomId);
        Task<List<PlaningStoryDal>> GetNotActualForRoomAsync(long roomId);
        Task<List<PlaningStoryDal>> GetNotActualStoriesAsync(long roomId
            , int pageNumber, int pageSize, List<long>  exclude);
        Task<long> GetCountNotActualForRoomAsync(long roomId, List<long> exclude);
        Task<PlaningStoryDal> UpdateAsync(long id, string name, string description);

    }
}
