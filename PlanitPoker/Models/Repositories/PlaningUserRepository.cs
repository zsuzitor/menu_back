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


        public PlaningUserRepository(MenuDbContext db):base(db)
        {
        }

    }
}
