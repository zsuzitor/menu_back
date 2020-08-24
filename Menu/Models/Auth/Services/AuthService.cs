﻿using jwtLib.JWTAuth.Interfaces;
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
        private readonly IJWTService _jwtService;

        private readonly MenuDbContext _db;


        private readonly string _userIdClaimName = "user_id";


        public AuthService(MenuDbContext db, IJWTHasher hasher, IJWTService jwtService)
        {
            _db = db;
            _hasher = hasher;
            _jwtService = jwtService;
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
            if (!(jwtUser is User user))
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
            var passwordHash = _hasher.GetHash(loginModel.Password);
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return null;
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == loginModel.Email && x.PasswordHash == passwordHash);


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

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            return await _jwtService.CreateAndSetNewTokensAsync(newUser);
        }

        public async Task<bool> LogOut(string accessToken)
        {
            var userId = _jwtService.GetUserIdFromAccessToken(accessToken);
            if (string.IsNullOrWhiteSpace(userId) || !long.TryParse(userId, out long userIdLong))
            {
                return false;
            }


            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userIdLong);
            user.RefreshTokenHash = null;
            await _db.SaveChangesAsync();
            return true;
        }


        public async Task<AllTokens> Refresh(string accessToken, string refreshToken)
        {
            var userId = _jwtService.GetUserIdFromAccessToken(accessToken);
            if (string.IsNullOrWhiteSpace(userId) || !long.TryParse(userId, out _))
            {
                return null;
            }

            return await _jwtService.RefreshAsync(userId, refreshToken);

        }


        #endregion IAuthService
    }
}
