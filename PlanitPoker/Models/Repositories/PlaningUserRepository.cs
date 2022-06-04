using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using PlanitPoker.Models.Repositories.Interfaces;

namespace PlanitPoker.Models.Repositories
{
    public sealed class PlaningUserRepository : GeneralRepository<PlaningRoomUserDal, long>, IPlaningUserRepository
    {


        public PlaningUserRepository(MenuDbContext db):base(db)
        {
        }

    }
}
