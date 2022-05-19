

using System.Linq;
using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PlanitPoker.Models.Repositories.Interfaces;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories
{
    public class RoomRepository : GeneralRepository<PlaningRoomDal, long>, IRoomRepository
    {


        public RoomRepository(MenuDbContext db) : base(db)
        {
        }


        public async Task<PlaningRoomDal> GetByName(string name)
        {
            return await _db.PlaningRooms.FirstOrDefaultAsync(x => x.Name == name.ToUpper());
        }

        public async Task<PlaningRoomDal> DeleteByName(string name)
        {
            var res = await GetByName(name);
            if (res != null)
            {
                _db.PlaningRooms.Remove(res);
                await _db.SaveChangesAsync();
            }

            return res;
        }


        public async Task LoadStories(PlaningRoomDal room)
        {
            if (room == null)
            {
                return;
            }

            _db.PlaningRooms.Attach(room);
            await _db.Entry(room).Collection(x => x.Stories).LoadAsync();
        }

        public async Task<bool> Exist(string name)
        {
            return (await _db.PlaningRooms.Where(x => x.Name == name.ToUpper()).Select(x => x.Id).FirstOrDefaultAsync()) != 0;
        }

        public async Task LoadUsers(PlaningRoomDal room)
        {
            if (room == null)
            {
                return;
            }

            _db.PlaningRooms.Attach(room);
            await _db.Entry(room).Collection(x => x.Users).LoadAsync();
        }
    }
}