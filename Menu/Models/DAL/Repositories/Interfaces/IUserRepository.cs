using Menu.Models.DAL.Domain;
using System.Threading.Tasks;

namespace Menu.Models.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByIdAndRefreshTokenAsync(long userId, string refreshTokenHash);
        Task<bool> RemoveRefreshTokenByOld(long userId, string refreshTokenHash);
        Task<bool> RemoveRefreshToken(long userId);
    }
}
