

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IStoryRepository : IGeneralRepository<PlaningStoryDal, long>
    {
        Task<List<PlaningStoryDal>> GetActualForRoom(long roomId);
        Task<List<PlaningStoryDal>> GetNotActualForRoom(long roomId);
        Task<PlaningStoryDal> UpdateAsync(long id, string name, string description);

    }
}
