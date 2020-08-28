using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Auth.Poco;
using Menu.Models.Healpers.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menu.Models.Healpers
{
    public class ApiHealper : IApiHealper
    {
        private readonly string _jsonCOntentType = "application/json";
        private readonly string _headerAccessToken = "Authorization_Access_Token";
        private readonly string _headerRefreshToken = "Authorization_Refresh_Token";


        public async Task WriteResponse<T>(HttpResponse response, T data)
        {
            response.ContentType = _jsonCOntentType;
            await response.WriteAsync(JsonSerializer.Serialize(data));
        }

        public UserInfo GetUserInfoFromRequest(HttpRequest request, IJWTService jwtService)
        {

            var accessToken = GetAccessTokenFromRequest(request);
            var userId = jwtService.GetUserIdFromAccessToken(accessToken);

            if (!long.TryParse(userId, out long userIdLong))
            {
                return null;
            }

            var res = new UserInfo(userIdLong)
            {
                RefreshToken = GetRefreshTokenFromRequest(request),
                AccessToken = accessToken
            };

            return res;
        }

        public string GetAccessTokenFromRequest(HttpRequest request)
        {
            return GetFromRequest(request, _headerAccessToken);
        }

        public string GetRefreshTokenFromRequest(HttpRequest request)
        {
            return GetFromRequest(request, _headerRefreshToken);
        }

        public void ClearUsersTokens(HttpResponse response)
        {

            response.Cookies.Delete(_headerAccessToken);
            response.Headers.Remove(_headerAccessToken);

            response.Cookies.Delete(_headerRefreshToken);
            response.Headers.Remove(_headerRefreshToken);
        }


        private string GetFromRequest(HttpRequest request, string key)
        {
            if (!request.Cookies.TryGetValue(key, out var authorizationToken) || !string.IsNullOrWhiteSpace(authorizationToken))
            {
                return authorizationToken;
            }

            if (request.Headers.TryGetValue(key, out var authorizationTokenValue))
            {
                return authorizationTokenValue;
            }
            return null;

        }

    }
}
