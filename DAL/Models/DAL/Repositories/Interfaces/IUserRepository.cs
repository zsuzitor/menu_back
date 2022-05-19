using BO.Models.DAL.Domain;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IUserRepository //: IGeneralRepository<User, long>
    {
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByIdNoTrackAsync(long userId);
        Task<User> GetUserByIdAndRefreshTokenAsync(long userId, string refreshTokenHash);
        Task<User> GetUserByIdAndRefreshTokenNoTrackAsync(long userId, string refreshTokenHash);
        Task<bool> RemoveRefreshTokenByOld(long userId, string refreshTokenHash);
        Task<bool> RemoveRefreshToken(long userId);
        Task<bool> SetRefreshTokenHashAsync(long userId, string refreshTokenHash);
        Task<User> GetByEmailAndPasswordHashAsync(string email, string passwordHash);
        Task<User> CreateNewAsync(User newUser);
        Task<bool> UserIsExist(string email, string login = null);
        Task<long?> GetIdByEmailAsync(string email);

        Task<User> GetShortInfo(long userId);
    }
}
