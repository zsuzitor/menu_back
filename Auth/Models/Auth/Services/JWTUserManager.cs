

using jwtLib.JWTAuth.Interfaces;
using BO.Models.DAL.Domain;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Menu.Models.Services.Interfaces;

namespace Auth.Models.Auth.Services
{
    public class JWTUserManager: IJWTUserManager
    {

        private readonly IJWTHasher _hasher;
        private readonly IUserService _userService;
        private readonly string _userIdClaimName = "user_id";

        public JWTUserManager(IJWTHasher hasher, IUserService userService)
        {
            //_db = db;
            _hasher = hasher;
            _userService = userService;
        }



        #region IJWTUserManager


        public async Task<bool> DeleteRefreshTokenFromUserAsync(string userId, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(refreshToken) || !long.TryParse(userId, out var longId))
            {
                return false;
            }

            return await _userService.RemoveRefreshTokenAsync(longId, refreshToken);
        }

        public async Task<bool> DeleteRefreshTokenFromUserAsync([NotNull] string userId)
        {
            if (!long.TryParse(userId, out var longId))
            {
                return false;
            }

            return await _userService.RemoveRefreshTokenAsync(longId);
        }

        public ClaimsIdentity GetIdentity(IJWTUser jwtUser, string authenticationType)
        {
            if (jwtUser == null)
                return null;

            var claims = new List<Claim>
            {
                //new Claim(type: _userIdClaimName,
                //    value: GetUserId(jwtUser)),
                //new Claim(type:ClaimTypes.Name,value:user.UserName)//,
                new Claim(type: ClaimsIdentity.DefaultRoleClaimType, value: "user")
            };

            claims.AddRange(GetIdentityForRefresh(jwtUser));

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, authenticationType, ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public List<Claim> GetIdentityForRefresh(IJWTUser jwtUser)
        {
            var claimId = new Claim(type: _userIdClaimName,
                    value: GetUserId(jwtUser));
            return new List<Claim>() { claimId };
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
            if (!(jwtUser is User user))
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

            return await _userService.GetUserByIdAndRefreshTokenHashAsync(longId, refreshTokenHash);
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
            if (!(jwtUser is User user))
            {
                return false;
            }
            return await _userService.SetRefreshTokenAsync(user.Id, refreshToken);

        }



        public async Task<IJWTUser> GetUserById([NotNull] string userId)
        {
            if (!long.TryParse(userId, out long userIdLong))
            {
                return null;
            }

            return await _userService.GetUserByIdAsync(userIdLong);
        }



        #endregion IJWTUserManager

    }
}
