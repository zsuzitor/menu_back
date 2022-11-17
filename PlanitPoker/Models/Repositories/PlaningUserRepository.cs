using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PlanitPoker.Models.Entity;
using PlanitPoker.Models.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories
{
    public sealed class PlaningUserRepository : GeneralRepository<PlaningRoomUserDal, long>, IPlaningUserRepository
    {


        public PlaningUserRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<List<RoomShortInfo>> GetRoomsAsync(long userId)
        {
            var g = _db.PlaningRoomUsers.Where(x => x.MainAppUserId == userId);
            //g.joi
            return await (_db.PlaningRoomUsers.Where(x => x.MainAppUserId == userId)
                //.Select(x=>x.RoomId)
                //.AsQueryable()
                .Join(_db.PlaningRooms, x => x.RoomId, x => x.Id
                , (x, y) => new RoomShortInfo() { Name = y.Name, ImagePath = y.ImagePath })).ToListAsync();
            //.Select(x => new RoomShortInfo() { Name = x.Name, ImagePath = }).ToListAsync();
        }

    }
}
