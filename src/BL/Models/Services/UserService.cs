using BO.Models.DAL.Domain;
using Common.Models.Error;
using Common.Models.Exceptions;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Menu.Models.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IJWTHasher _hasher;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly IDBHelper _dbHelper;
        private readonly MenuDbContext _db;


        public UserService(IUserRepository userRepository, IJWTHasher hasher
            , IImageService imageService, IDBHelper dbHelper, MenuDbContext db)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _imageService = imageService;
            _dbHelper = dbHelper;
            _db = db;
        }


        public async Task<User> GetUserByIdAndRefreshTokenHashAsync(long userId, string refreshTokenHash)
        {
            return await _userRepository.GetUserByIdAndRefreshTokenNoTrackAsync(userId, refreshTokenHash);
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            return await _userRepository.GetUserByIdNoTrackAsync(userId);
        }

        public async Task<bool> RemoveRefreshTokenAsync(long userId, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            var tokenHash = _hasher.GetHash(refreshToken);

            return await _userRepository.RemoveRefreshTokenByOld(userId, tokenHash);

        }


        public async Task<bool> RemoveRefreshTokenAsync(long userId)
        {
            return await _userRepository.RemoveRefreshToken(userId);
        }

        public async Task<bool> SetRefreshTokenAsync(long userId, string refreshToken)
        {
            var hash = _hasher.GetHash(refreshToken);
            return await _userRepository.SetRefreshTokenHashAsync(userId, hash);
        }

        public async Task<User> GetByEmailAndPasswordAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = await _userRepository.GetUserAsync(email);
            if (!_hasher.VerifySaltHash(password, user?.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<long?> GetIdByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            return await _userRepository.GetIdByEmailAsync(email);
        }

        public async Task<User> CreateNewAsync(User newUser)
        {
            try
            {
                if (await _userRepository.UserIsExist(newUser.Email, newUser.Login))
                {
                    throw new SomeCustomException(ErrorConsts.UserAlreadyExist);
                }

                return await _userRepository.CreateNewAsync(newUser);
            }
            catch (SomeCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                var newE = new SomeCustomException(ErrorConsts.CanNotRegister, "Ошибка при регистрации пользователя", e);
                throw newE;
            }

        }


        public async Task<User> GetShortInfoAsync(long userId)
        {
            var res = await _userRepository.GetShortInfo(userId);
            if (res == null)
            {
                throw new NotAuthException();//todo понять  можно ли убрать это, тк оно тут лишнее
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return res;
        }

        public async Task<User> UpdateUserPasswordAsync(long userId, string password)
        {
            var passwordHash = _hasher.GetSecuredHash(password);
            return await _userRepository.UpdateUserPasswordAsync(userId, passwordHash);
        }

        public async Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                throw new SomeCustomException(ErrorConsts.BadPasswords);
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            if (!_hasher.VerifySaltHash(oldPassword, user?.PasswordHash))
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            var passwordHash = _hasher.GetSecuredHash(newPassword);

            user = await _userRepository.UpdateUserPasswordAsync(userId, passwordHash);
            if (user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return true;
        }

        public async Task<bool> ChangeNameAsync(long userId, string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new SomeCustomException(ErrorConsts.BadName);
            }

            var user = await _userRepository.UpdateUserNameAsync(userId, newName);
            if (user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return true;
        }

        public async Task<string> ChangeImageAsync(long userId, IFormFile image)
        {
            //var user = await _userRepository.GetUserByIdAsync(userId);
            //if (user == null)
            //{
            //    throw new SomeCustomException(ErrorConsts.NotFound);
            //}
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            string newPath = null;
            await _dbHelper.ActionInTransaction(_db, async () =>
            {

                CustomImage imageRecord = null;
                if (image != null)
                {
                    //throw new SomeCustomException(ErrorConsts.FileError);
                    imageRecord = await _imageService.Upload(image);
                }

                //var oldImage = await _imageService.GetById(user.ImageId);
                if (user.ImageId.HasValue)
                    await _imageService.DeleteById(user.ImageId.Value);

                user.ImageId = imageRecord?.Id;
                _ = await _userRepository.UpdateAsync(user);
                newPath = imageRecord?.Path;
            });
            return newPath;

        }

    }
}
