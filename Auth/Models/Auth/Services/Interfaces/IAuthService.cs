
using jwtLib.JWTAuth.Models.Poco;
using System.Threading.Tasks;
using Auth.Models.Auth.Poco.Input;

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
