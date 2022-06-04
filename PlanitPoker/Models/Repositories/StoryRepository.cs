

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PlanitPoker.Models.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories
{
    public sealed class StoryRepository : GeneralRepository<PlaningStoryDal, long>, IStoryRepository
    {


        public StoryRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<List<PlaningStoryDal>> GetActualForRoom(long roomId)
        {
            return await _db.PlaningStories.AsNoTracking().Where(x => x.RoomId == roomId && !x.Completed).ToListAsync();
        }

        public async Task<List<PlaningStoryDal>> GetNotActualForRoom(long roomId)
        {
            return await _db.PlaningStories.AsNoTracking().Where(x => x.RoomId == roomId && x.Completed).ToListAsync();
        }
    }
}
