using Common.Models;
using System.Security.Claims;

namespace Auth.Models.Auth
{
    public static class AuthExtension
    {
        public static long GetUserId(this ClaimsPrincipal claims)
        {
            var userId = claims.FindFirstValue(Constants.Claims.Id);
            long.TryParse(userId, out long userIdLong);
            return userIdLong;
        }
    }
}
