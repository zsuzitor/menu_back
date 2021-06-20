

using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using PlanitPoker.Models.Repositories.Interfaces;

namespace PlanitPoker.Models.Repositories
{
    public class StoryRepository : GeneralRepository<PlaningStoryDal, long>, IStoryRepository
    {
        private readonly MenuDbContext _db;


        public StoryRepository(MenuDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
