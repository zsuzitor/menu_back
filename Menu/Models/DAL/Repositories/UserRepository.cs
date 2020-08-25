﻿

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

        public async Task<bool> SetRefreshTokenHashAsync(long userId, string refreshTokenHash)
        {
            var userFromDb = await _db.Users.FirstOrDefaultAsync(x1 => x1.Id == userId);
            if (userFromDb == null)
            {
                return false;
            }

            userFromDb.RefreshTokenHash = refreshTokenHash;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetByEmailAndPasswordHashAsync(string email, string passwordHash)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
        }

        public async Task<User> CreateNewAsync(User newUser)
        {
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return newUser;
        }

    }
}
