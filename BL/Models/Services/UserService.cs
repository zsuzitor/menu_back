using jwtLib.JWTAuth.Interfaces;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Common.Models.Exceptions;
using Menu.Models.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Common.Models.Error;
using Microsoft.AspNetCore.Http;

namespace Menu.Models.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IJWTHasher _hasher;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;


        public UserService(IUserRepository userRepository, IJWTHasher hasher
            , IImageService imageService)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _imageService = imageService;
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
            var passwordHash = _hasher.GetHash(password);
            return await _userRepository.UpdateUserPasswordAsync(userId, passwordHash);
        }

        public async Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                throw new SomeCustomException(ErrorConsts.BadPasswords);
            }

            var oldPasswordHash = _hasher.GetHash(oldPassword);
            var passwordHash = _hasher.GetHash(newPassword);
            var user = await _userRepository.UpdateUserPasswordAsync(userId, oldPasswordHash, passwordHash);
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
            string pathImage = null;
            if (image != null)
            {
                //throw new SomeCustomException(ErrorConsts.FileError);
                pathImage = await _imageService.CreateUploadFileWithOutDbRecord(image);
                if (string.IsNullOrEmpty(pathImage))
                {
                    throw new SomeCustomException(ErrorConsts.FileError);
                }
            }
            

            var user = await _userRepository.UpdateImageAsync(userId, pathImage);
            if (user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return pathImage;

        }

    }
}
