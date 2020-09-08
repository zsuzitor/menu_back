
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Auth.Poco;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menu.Models.Healpers.Interfaces
{
    public interface IApiHealper
    {
        Task WriteResponseAsync<T>(HttpResponse response, T data);
        string GetAccessTokenFromRequest(HttpRequest request);
        string GetRefreshTokenFromRequest(HttpRequest request);
        void ClearUsersTokens(HttpResponse response);
        UserInfo GetUserInfoFromRequest(HttpRequest request, IJWTService jwtService);
        UserInfo CheckAuthorized(HttpRequest request, IJWTService jwtService, bool withError = false);

        Task DoStandartSomething(Func<Task> action, HttpResponse response, ILogger logger);
    }
}
