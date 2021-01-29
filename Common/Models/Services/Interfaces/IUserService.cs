

using Menu.Models.DAL.Domain;
using System.Threading.Tasks;

namespace Common.Models.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(long userId);
        Task<bool> RemoveRefreshToken(long userId,string refreshToken);
        Task<bool> RemoveRefreshToken(long userId);
        Task<User> GetUserByIdAndRefreshTokenHashAsync(long userId, string refreshTokenHash);
        Task<bool> SetRefreshTokenAsync(long userId, string refreshToken);
        Task<User> GetByEmailAndPasswordAsync(string email, string password);
        Task<User> CreateNewAsync(User newUser);
    }
}
