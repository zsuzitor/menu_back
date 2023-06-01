

using DAL.Models.DAL.Repositories.Interfaces;
using Common.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

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

        public async Task<User> GetUserByIdNoTrackAsync(long userId)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<List<(long userId, string email)>> GetUserEmailsAsync(List<long> userId)
        {
            var res = await _db.Users.AsNoTracking().Where(x => userId.Contains(x.Id))
                .Select(x => new { x.Id, x.Email }).ToListAsync();
            return res.Select(x => (x.Id, x.Email)).ToList();
            //new User() { Id = x.Id, Email = x.Email }
        }

        public async Task<List<(long userId, string email)>> GetIdByEmailAsync(List<string> email)
        {
            return (await _db.Users.AsNoTracking()
                .Where(x => email.Contains(x.Email))
                .Select(x => new { Id = x.Id, Email = x.Email })
                .ToListAsync())
                .Select(x => (x.Id, x.Email)).ToList();
        }

        public async Task<User> GetUserByIdAndRefreshTokenAsync(long userId, string refreshTokenHash)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == userId && x.RefreshTokenHash == refreshTokenHash);
        }

        public async Task<User> GetUserByIdAndRefreshTokenNoTrackAsync(long userId, string refreshTokenHash)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId && x.RefreshTokenHash == refreshTokenHash);
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
            return await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == passwordHash);
        }

        public async Task<bool> UserIsExist(string email, string login = null)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(login))
            {
                throw new SomeCustomException("не передан логин или почта");
            }

            //return await _db.Users.AnyAsync(x => 
            //    x.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase) 
            //        || x.Login.Equals(login, System.StringComparison.OrdinalIgnoreCase));
            return await _db.Users.AnyAsync(x => x.Email == email || x.Login == login);
        }

        public async Task<long?> GetIdByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new SomeCustomException("не передана почта");
            }

            return (await _db.Users.FirstOrDefaultAsync(x => x.Email == email))?.Id;
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
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            user.Login = null;
            user.PasswordHash = null;
            user.RefreshTokenHash = null;
            return user;
        }

        public async Task<User> UpdateUserPasswordAsync(long userId, string passwordHash)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            
            if(user != null)
            {
                user.PasswordHash = passwordHash;
                await _db.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> UpdateUserPasswordAsync(long userId, string oldPasswordHash, string passwordHash)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId
                && x.PasswordHash == oldPasswordHash);
            if (user != null)
            {
                user.PasswordHash = passwordHash;
                await _db.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> UpdateUserNameAsync(long userId, string newName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                user.Name = newName;
                await _db.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> UpdateImageAsync(long userId, string imagePath)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                user.ImagePath = imagePath;
                await _db.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> UpdateAsync(User newUser)
        {
            await _db.SaveChangesAsync();
            return newUser;
        }
    }
}
