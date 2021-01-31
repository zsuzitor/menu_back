using jwtLib.JWTAuth.Interfaces;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Common.Models.Exceptions;
using Menu.Models.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Menu.Models.Services
{
    public class UserService : IUserService
    {
        private readonly IJWTHasher _hasher;
        private readonly IUserRepository _userRepository;

        //private readonly MenuDbContext _db;


        public UserService(IUserRepository userRepository, IJWTHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }


        public async Task<User> GetUserByIdAndRefreshTokenHashAsync(long userId, string refreshTokenHash)
        {
            return await _userRepository.GetUserByIdAndRefreshTokenAsync(userId, refreshTokenHash);
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<bool> RemoveRefreshToken(long userId, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            var tokenHash = _hasher.GetHash(refreshToken);

            return await _userRepository.RemoveRefreshTokenByOld(userId, tokenHash);

        }


        public async Task<bool> RemoveRefreshToken(long userId)
        {
            return await _userRepository.RemoveRefreshToken(userId);
        }

        public async Task<bool> SetRefreshTokenAsync(long userId, string refreshToken)
        {
            var hash = _hasher.GetHash(refreshToken);
            return await _userRepository.SetRefreshTokenHashAsync(userId, hash);
        }

        private async Task<User> GetByEmailAndPasswordHashAsync(string email, string passwordHash)
        {
            return await _userRepository.GetByEmailAndPasswordHashAsync(email, passwordHash);
        }

        public async Task<User> GetByEmailAndPasswordAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var passwordHash = _hasher.GetHash(password);
            return await GetByEmailAndPasswordHashAsync(email, passwordHash);
        }

        public async Task<User> CreateNewAsync(User newUser)
        {
            try
            {
                if (await _userRepository.UserIsExist(newUser.Email, newUser.Login))
                {
                    throw new SomeCustomException("user_already_exist");
                }
                return await _userRepository.CreateNewAsync(newUser);
            }
            catch (SomeCustomException)
            {
                throw;
            }
            catch(Exception e)
            {
                var newE =new SomeCustomException("can_not_register","Ошибка при регистрации пользователя", e);
                throw newE;
            }
            
        }

    }
}
