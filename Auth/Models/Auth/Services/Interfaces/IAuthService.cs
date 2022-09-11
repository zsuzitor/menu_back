
using jwtLib.JWTAuth.Models.Poco;
using System.Threading.Tasks;
using Auth.Models.Auth.Poco.Input;

namespace Auth.Models.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AllTokens> LoginAsync(LoginModel loginModel);
        Task<AllTokens> RegisterAsync(RegisterModel registerModel);
        Task<bool> LogOutAsync(string accessToken);

        Task<AllTokens> RefreshAsync(string accessToken, string refreshToken);
        Task<bool> SendMessageForgotPasswordAsync(string email);
        Task<bool> CheckRecoverPasswordCodeAsync(string code);
        Task<bool> RecoverPasswordAsync(string code, string newPassword);
    }
}
