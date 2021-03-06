﻿

using DAL.Models.DAL.Repositories.Interfaces;
using Common.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BO.Models.DAL.Domain;

namespace DAL.Models.DAL.Repositories
{
    public class UserRepository : IUserRepository
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

        public async Task<User> GetUserByIdAndRefreshTokenAsync(long userId, string refreshTokenHash)
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
            //var g= _db.Users.ToList();
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
        }

        public async Task<bool> UserIsExist(string email, string login = null)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(login))
            {
                throw new SomeCustomException("не передан логин или почта");
            }

            return await _db.Users.AnyAsync(x => x.Email == email || x.Login == login);
        }

        public async Task<User> CreateNewAsync(User newUser)
        {
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> GetShortInfo(long userId)
        {
            //TODO тут бы обрезать модель, грузить только то что надо и тд, но пока что так
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.Login = null;
            user.PasswordHash = null;
            user.RefreshTokenHash = null;
            return user;
        }
    }
}
