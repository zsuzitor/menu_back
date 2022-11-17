

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using PlanitPoker.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IPlaningUserRepository : IGeneralRepository<PlaningRoomUserDal, long>
    {
        //Task<List<PlaningRoomUserDal>> GetForRoom(long roomId);
        //Task<PlaningRoomUserDal> GetByMainAppId(string roomName, long mainAppUserId);
        Task<List<RoomShortInfo>> GetRoomsAsync(long userId);
    }
}
