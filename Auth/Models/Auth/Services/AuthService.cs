using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models.Poco;
using Auth.Models.Auth.Services.Interfaces;
using BO.Models.DAL.Domain;
using Common.Models.Error;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using System.Threading.Tasks;
using Menu.Models.Services.Interfaces;
using Auth.Models.Auth.Poco.Input;

namespace Auth.Models.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJWTHasher _hasher;
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        private readonly IErrorService _errorService;




        public AuthService(IJWTHasher hasher, IUserService userService, IJWTService jwtService,IErrorService errorService)
        {
            _hasher = hasher;
            _jwtService = jwtService;
            _userService = userService;
            _errorService = errorService;
        }



        ///---------------------------------------------------------IAuthService-------------
        ///
        #region IAuthService



        public async Task<AllTokens> Login(LoginModel loginModel)
        {

            var user = await _userService.GetByEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
            if(user == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }
            return await _jwtService.CreateAndSetNewTokensAsync(user);

            //
        }

        public async Task<AllTokens> Register(RegisterModel registerModel)
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

        public async Task<bool> LogOut(string accessToken)
        {
            var userId = _jwtService.GetUserIdFromAccessTokenIfCan(accessToken);
            if (string.IsNullOrWhiteSpace(userId) || !long.TryParse(userId, out long userIdLong))
            {
                return false;
            }

            return await _userService.RemoveRefreshTokenAsync(userIdLong);
        }


        public async Task<AllTokens> Refresh(string accessToken, string refreshToken)
        {
            var userId = _jwtService.GetUserIdFromAccessTokenIfCan(accessToken);
            if (string.IsNullOrWhiteSpace(userId) || !long.TryParse(userId, out _))
            {
                return null;
            }

            return await _jwtService.RefreshAsync(userId, refreshToken);

        }




        #endregion IAuthService
    }
}
