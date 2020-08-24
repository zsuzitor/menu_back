
using jwtLib.JWTAuth.Models.Poco;
using Menu.Models.Auth.InputModels;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Menu.Models.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AllTokens> Login(LoginModel loginModel);
        Task<AllTokens> Register(RegisterModel registerModel);
        Task<bool> LogOut(string accessToken);

        Task<AllTokens> Refresh(string accessToken,string refreshToken);
    }
}
