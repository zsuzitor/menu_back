using jwtLib.JWTAuth.Interfaces;
using Menu.Models.DAL;
using Menu.Models.DAL.Domain;
using Menu.Models.DAL.Repositories.Interfaces;
using Menu.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menu.Models.Services
{
    public class UserService : IUserService
    {
        private readonly IJWTHasher _hasher;
        private readonly IUserRepository _userRepository;

        //private readonly MenuDbContext _db;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<User> GetUserByIdAndRefreshTokenHashAsync(long userId, string refreshTokenHash)
        {
            return await _userRepository.GetUserByIdAndRefreshTokenAsync(userId,refreshTokenHash);
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<bool> RemoveRefreshToken(long userId, string refreshToken)
        {
            if ( string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            var tokenHash = _hasher.GetHash(refreshToken);

            return await _userRepository.RemoveRefreshTokenByOld(userId,tokenHash);
           
        }


        public async Task<bool> RemoveRefreshToken(long userId)
        {
            return await _userRepository.RemoveRefreshToken(userId);
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
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == loginModel.Email && x.PasswordHash == passwordHash); 
        }

        public async Task<User> CreateNewAsync(User newUser)
        {
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return newUser;
        }

    }
}
