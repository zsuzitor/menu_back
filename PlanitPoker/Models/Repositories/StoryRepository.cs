

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

        public async Task<PlaningStoryDal> UpdateAsync(long id, string name, string description)
        {
            var rec = await _db.PlaningStories.FirstOrDefaultAsync(x => x.Id == id);
            if (rec == null)
            {
                return null;
            }

            rec.Description = description;
            rec.Name = name;
            return await UpdateAsync(rec);
        }
    }
}
