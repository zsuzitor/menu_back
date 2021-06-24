using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PlanitPoker.Models.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories
{
    public class PlaningUserRepository : GeneralRepository<PlaningRoomUserDal, long>, IPlaningUserRepository
    {
        private readonly MenuDbContext _db;


        public PlaningUserRepository(MenuDbContext db):base(db)
        {
            _db = db;
        }

        //public async Task<PlaningRoomUserDal> GetByMainAppId(string roomName, long mainAppUserId)
        //{
        //    return await _db.PlaningRoomUsers.FirstOrDefaultAsync(x => x.room == roomName);
        //}

        //public async Task<List<PlaningRoomUserDal>> GetForRoom(string roomName)
        //{

        //}
    }
}
