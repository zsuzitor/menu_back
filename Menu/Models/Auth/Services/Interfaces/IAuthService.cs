
using jwtLib.JWTAuth.Models.Poco;
using Menu.Models.Auth.InputModels;
using System.Threading.Tasks;

namespace Menu.Models.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AllTokens> Login(LoginModel loginModel);
        Task<AllTokens> Register(RegisterModel registerModel);
        Task<AllTokens> LogOut();
    }
}
