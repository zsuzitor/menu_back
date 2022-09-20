using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models.Poco;
using Auth.Models.Auth.Services.Interfaces;
using BO.Models.DAL.Domain;
using Common.Models.Error;
using Common.Models.Exceptions;
using System.Threading.Tasks;
using Menu.Models.Services.Interfaces;
using Auth.Models.Auth.Poco.Input;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace Auth.Models.Auth.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IJWTHasher _hasher;
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly AuthEmailService _authEmailService;


        private readonly string _userIdClaimName = "user_id";


        public AuthService(IJWTHasher hasher, IUserService userService
            , IJWTService jwtService, ITokenHandler tokenHandler
            , IConfiguration configuration
            , AuthEmailService authEmailService)
        {
            _hasher = hasher;
            _jwtService = jwtService;
            _userService = userService;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _authEmailService = authEmailService;
        }



        ///---------------------------------------------------------IAuthService-------------
        ///
        #region IAuthService



        public async Task<AllTokens> LoginAsync(LoginModel loginModel)
        {

            var user = await _userService.GetByEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
            if (user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }
            return await _jwtService.CreateAndSetNewTokensAsync(user);

            //
        }

        public async Task<AllTokens> RegisterAsync(RegisterModel registerModel)
        {
            var passwordHash = _hasher.GetHash(registerModel.Password);
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return null;
            }

            var newUser = new User()
            {
                Email = registerModel.Email,
                Login = registerModel.Email,
                PasswordHash = passwordHash
            };

            newUser = await _userService.CreateNewAsync(newUser);

            return await _jwtService.CreateAndSetNewTokensAsync(newUser);
        }

        public async Task<bool> LogOutAsync(string accessToken)
        {
            var userId = _jwtService.GetUserIdFromAccessTokenIfCan(accessToken);
            if (string.IsNullOrWhiteSpace(userId) || !long.TryParse(userId, out long userIdLong))
            {
                return false;
            }

            return await _userService.RemoveRefreshTokenAsync(userIdLong);
        }


        public async Task<AllTokens> RefreshAsync(string accessToken, string refreshToken)
        {
            var userId = _jwtService.GetUserIdFromAccessTokenIfCan(accessToken);
            if (string.IsNullOrWhiteSpace(userId) || !long.TryParse(userId, out _))
            {
                return null;
            }

            return await _jwtService.RefreshAsync(userId, refreshToken);

        }

        public async Task<bool> SendMessageForgotPasswordAsync(string email)
        {
            //тут не должно быть исключений, тк мы всегда(почти) должны сказать что 
            //ЕСЛИ почта указана верно, сообщение было отправлено
            var key = _configuration["RestorePasswordTokenKey"];

            //_tokenHandler.DecodeToken
            //_JWTUserManager.ItIsUserClaims;

            var userId = await _userService.GetIdByEmailAsync(email);
            if (userId == null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(type: ClaimsIdentity.DefaultRoleClaimType, value: "user"),
                new Claim(type: _userIdClaimName, value: userId.Value.ToString()),
            };

            if (!int.TryParse(_configuration["RestorePasswordTokenLifeTimeMinutes"], out var tokenLifeTime))
            {
                tokenLifeTime = 5;
            }

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "password_restore");
            var token = _tokenHandler.GenerateToken(claimsIdentity, tokenLifeTime, key);
            await _authEmailService.SendEmailAsync(email, "Восстановление пароля", token);
            return true;
        }

        public async Task<bool> CheckRecoverPasswordCodeAsync(string code)
        {
            var userId = GetUserIdFromRecoverPasswordCode(code);
            return !string.IsNullOrWhiteSpace(userId);
        }

        public async Task<bool> RecoverPasswordAsync(string code, string newPassword)
        {
            var userId = GetUserIdFromRecoverPasswordCode(code);
            if (string.IsNullOrWhiteSpace(userId) || !long.TryParse(userId, out var userIdLong))
            {
                throw new SomeCustomException(AuthConst.AuthErrors.ProblemWithRecoverPasswordToken);
            }

            var user = await _userService.UpdateUserPasswordAsync(userIdLong, newPassword);
            if (user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return true;
        }

        private string GetUserIdFromRecoverPasswordCode(string code)
        {
            var key = _configuration["RestorePasswordTokenKey"];
            string userId = null;
            try
            {
                var principal = _tokenHandler.GetClaimsFromToken(code, key, out _);
                //SecurityTokenExpiredException

                var claims = principal?.Identities?.SelectMany(x => x.Claims);
                userId = claims.FirstOrDefault(x => string.Equals(x.Type, _userIdClaimName))?.Value;
            }
            //SecurityTokenExpiredException
            catch
            {
                throw new SomeCustomException(AuthConst.AuthErrors.ProblemWithRecoverPasswordToken);
            }

            return userId;
        }



        #endregion IAuthService
    }
}