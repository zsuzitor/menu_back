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
            if (!request.Cookies.TryGetValue("Authorization_Access_Token", out var authorizationToken) || !string.IsNullOrWhiteSpace(authorizationToken))
            {
                return authorizationToken;
            }

            if (request.Headers.TryGetValue("Authorization_Access_Token", out var authorizationTokenValue))
            {
                return authorizationTokenValue;
            }
            return null;

        }
    }
}
