

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IRoomRepository : IGeneralRepository<PlaningRoomDal, long>
    {
        Task<PlaningRoomDal> GetByNameAsync(string name);
        Task<bool> ExistAsync(string name);
        Task<PlaningRoomDal> DeleteByNameAsync(string name);
        Task LoadStoriesAsync(PlaningRoomDal room);
        Task LoadUsersAsync(PlaningRoomDal room);
    }
}
