

using BO.Models.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(long userId);
        Task<User> UpdateUserPasswordAsync(long userId, string password);
        Task<bool> RemoveRefreshTokenAsync(long userId,string refreshToken);
        Task<bool> RemoveRefreshTokenAsync(long userId);
        Task<User> GetUserByIdAndRefreshTokenHashAsync(long userId, string refreshTokenHash);
        Task<bool> SetRefreshTokenAsync(long userId, string refreshToken);
        Task<User> GetByEmailAndPasswordAsync(string email, string password);
        Task<long?> GetIdByEmailAsync(string email);

        Task<User> CreateNewAsync(User newUser);
        Task<User> GetShortInfoAsync(long userId);
        Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword);
        Task<bool> ChangeNameAsync(long userId, string newName);
        Task<string> ChangeImageAsync(long userId, IFormFile image);

    }
}
