using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using PlanitPoker.Models.Repositories.Interfaces;

namespace PlanitPoker.Models.Repositories
{
    public class PlaningUserRepository : GeneralRepository<PlaningRoomUserDal, long>, IPlaningUserRepository
    {
        private readonly MenuDbContext _db;


        public PlaningUserRepository(MenuDbContext db):base(db)
        {
            _db = db;
        }

    }
}
