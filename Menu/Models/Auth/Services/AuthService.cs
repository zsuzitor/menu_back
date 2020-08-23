using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models.Poco;
using Menu.Models.Auth.InputModels;
using Menu.Models.Auth.Services.Interfaces;
using Menu.Models.DAL;
using Menu.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Menu.Models.Auth.Services
{
    public class AuthService : IAuthService, IJWTUserManager
    {
        private readonly IJWTHasher _hasher;
        private readonly MenuDbContext _db;


        private readonly string _userIdClaimName = "user_id";


        public AuthService(MenuDbContext db, IJWTHasher hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        #region IJWTUserManager
        public async Task<bool> DeleteRefreshTokenFromUserAsync(string userId, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(refreshToken) || !long.TryParse(userId, out var longId))
            {
                return false;
            }

            var tokenHash = _hasher.GetHash(refreshToken);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == longId && x.RefreshTokenHash == tokenHash);
            if (user == null)
            {
                return false;
            }

            user.RefreshTokenHash = null;
            await _db.SaveChangesAsync();
            return true;
        }

        public ClaimsIdentity GetIdentity(IJWTUser jwtUser, string authenticationType)
        {
            if (jwtUser == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(type: _userIdClaimName,
                    value: GetUserId(jwtUser)),
                //new Claim(type:ClaimTypes.Name,value:user.UserName)//,
                new Claim(type: ClaimsIdentity.DefaultRoleClaimType, value: "user")
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, authenticationType, ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public List<Claim> GetIdentityForRefresh(IJWTUser jwtUser)
        {
            throw new System.NotImplementedException();
        }

        public string GetIdFromClaims(ClaimsPrincipal claims)
        {
            return GetIdFromClaims(claims.Claims);
        }

        public string GetIdFromClaims(IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(x1 => x1.Type == _userIdClaimName)?.Value;
        }

        public string GetUserId(IJWTUser jwtUser)
        {
            var user = jwtUser as User;
            if (user == null)
            {
                return null;
            }
            return user.Id.ToString();

        }

        public async Task<IJWTUser> GetWithRefreshTokenAsync(string userId, string refreshToken)
        {
            var tokenHash = _hasher.GetHash(refreshToken);
            return await GetWithRefreshTokenHashAsync(userId, tokenHash);
        }

        public async Task<IJWTUser> GetWithRefreshTokenHashAsync(string userId, string refreshTokenHash)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(refreshTokenHash) || !long.TryParse(userId, out var longId))
            {
                return null;
            }

            return await _db.Users.FirstOrDefaultAsync(x => x.Id == longId && x.RefreshTokenHash == refreshTokenHash);
        }

        public bool ItIsUserClaims(IEnumerable<Claim> claims, IJWTUser jwtUser)
        {
            var userId = GetUserId(jwtUser);

            return ItIsUserClaims(claims, userId);
        }

        public bool ItIsUserClaims(IEnumerable<Claim> claims, string userId)
        {
            var claimId = claims?.FirstOrDefault(x => x.Type == _userIdClaimName);
            if (claimId == null || userId == null)
            {
                return false;
            }

            if (userId != claimId.Value)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SetRefreshTokenAsync(IJWTUser jwtUser, string refreshToken)
        {
            var tokenHash = _hasher.GetHash(refreshToken);
            return await SetRefreshTokenHashAsync(jwtUser, tokenHash);
        }

        public async Task<bool> SetRefreshTokenHashAsync(IJWTUser jwtUser, string refreshTokenHash)
        {
            var user = jwtUser as User;
            if (user == null)
            {
                return false;
            }

            var userFromDb = await _db.Users.FirstOrDefaultAsync(x1 => x1.Id == user.Id);
            if (userFromDb == null)
            {
                return false;
            }

            userFromDb.RefreshTokenHash = refreshTokenHash;
            await _db.SaveChangesAsync();
            return true;
        }



        #endregion IJWTUserManager

        ///---------------------------------------------------------IAuthService-------------
        ///
        #region IAuthService
        public async Task<AllTokens> Login(LoginModel loginModel)
        {

        }

        public async Task<AllTokens> Register(RegisterModel registerModel)
        {

        }

        public async Task<AllTokens> LogOut(string accessToken)
        {

        }

        #endregion IAuthService
    }
}
