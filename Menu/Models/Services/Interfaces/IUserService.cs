

using Menu.Models.DAL.Domain;
using System.Threading.Tasks;

namespace Menu.Models.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(long userId);
        Task<bool> RemoveRefreshToken(long userId,string refreshToken);
        Task<bool> RemoveRefreshToken(long userId);
        Task<User> GetUserByIdAndRefreshTokenHashAsync(long userId, string refreshTokenHash);
        Task<bool> SetRefreshTokenHashAsync(long userId, string refreshTokenHash);
        Task<User> GetByEmailAndPasswordHashAsync(string email,string passwordHash);
        Task<User> CreateNewAsync(User newUser);
    }
}
