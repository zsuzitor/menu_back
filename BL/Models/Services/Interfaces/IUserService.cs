

using BO.Models.DAL.Domain;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(long userId);
        Task<bool> RemoveRefreshTokenAsync(long userId,string refreshToken);
        Task<bool> RemoveRefreshTokenAsync(long userId);
        Task<User> GetUserByIdAndRefreshTokenHashAsync(long userId, string refreshTokenHash);
        Task<bool> SetRefreshTokenAsync(long userId, string refreshToken);
        Task<User> GetByEmailAndPasswordAsync(string email, string password);
        Task<long?> GetIdByEmailAsync(string email);

        Task<User> CreateNewAsync(User newUser);
        Task<User> GetShortInfoAsync(long userId);
    }
}
