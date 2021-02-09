using BO.Models.DAL.Domain;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByIdAndRefreshTokenAsync(long userId, string refreshTokenHash);
        Task<bool> RemoveRefreshTokenByOld(long userId, string refreshTokenHash);
        Task<bool> RemoveRefreshToken(long userId);
        Task<bool> SetRefreshTokenHashAsync(long userId, string refreshTokenHash);
        Task<User> GetByEmailAndPasswordHashAsync(string email, string passwordHash);
        Task<User> CreateNewAsync(User newUser);
        Task<bool> UserIsExist(string email, string login = null);


    }
}
