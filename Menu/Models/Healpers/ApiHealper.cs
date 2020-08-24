using Menu.Models.Healpers.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menu.Models.Healpers
{
    public class ApiHealper : IApiHealper
    {
        public async Task WriteResponse<T>(HttpResponse response, T data)
        {
            response.ContentType = "application/json";
            await response.WriteAsync(JsonSerializer.Serialize(data));
        }

        public string GetAccessTokenFromRequest(HttpRequest request)
        {
            return GetFromRequest(request, "Authorization_Access_Token");
        }

        public string GetRefreshTokenFromRequest(HttpRequest request)
        {
            return GetFromRequest(request, "Authorization_Refresh_Token");
        }

        public void ClearUsersTokens(HttpResponse response)
        {

            response.Cookies.Delete("Authorization_Access_Token");
            response.Headers.Remove("Authorization_Access_Token");

            response.Cookies.Delete("Authorization_Refresh_Token");
            response.Headers.Remove("Authorization_Refresh_Token");
        }


        private string GetFromRequest(HttpRequest request,string key)
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
