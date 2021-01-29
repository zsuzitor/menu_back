
using jwtLib.JWTAuth.Models.Poco;
using Auth.Models.Auth.InputModels;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Auth.Models.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AllTokens> Login(LoginModel loginModel);
        Task<AllTokens> Register(RegisterModel registerModel);
        Task<bool> LogOut(string accessToken);

        Task<AllTokens> Refresh(string accessToken,string refreshToken);
    }
}
