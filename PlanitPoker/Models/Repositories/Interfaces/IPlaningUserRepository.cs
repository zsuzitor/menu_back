﻿

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL.Repositories.Interfaces;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IPlaningUserRepository : IGeneralRepository<PlaningRoomUserDal, long>
    {
        //Task<List<PlaningRoomUserDal>> GetForRoom(long roomId);
        //Task<PlaningRoomUserDal> GetByMainAppId(string roomName, long mainAppUserId);

    }
}
