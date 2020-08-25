

using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Menu.Models.DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly MenuDbContext _db;
        public UserRepository(MenuDbContext db)
        {
            _db = db;
        }


        public async Task<User> GetUserByIdAsync(long userId)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User> GetUserByIdAndRefreshTokenAsync(long userId,string refreshTokenHash)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == userId && x.RefreshTokenHash == refreshTokenHash);
        }

        public async Task<bool> RemoveRefreshTokenByOld(long userId, string refreshTokenHash)
        {
            var user = await GetUserByIdAndRefreshTokenAsync(userId, refreshTokenHash);
            if (user == null)
            {
                return false;
            }

            user.RefreshTokenHash = null;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRefreshToken(long userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.RefreshTokenHash = null;
            await _db.SaveChangesAsync();
            return true;
        }


        


    }
}
