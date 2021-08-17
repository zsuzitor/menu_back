

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IRoomRepository : IGeneralRepository<PlaningRoomDal, long>
    {
        Task<PlaningRoomDal> GetByName(string name);
        Task<bool> Exist(string name);
        Task<PlaningRoomDal> DeleteByName(string name);
        Task LoadStories(PlaningRoomDal room);
        Task LoadUsers(PlaningRoomDal room);
    }
}
