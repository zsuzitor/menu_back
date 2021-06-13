
using BO.Models.Auth;
using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models.Poco;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WEB.Common.Models.Helpers.Interfaces
{
    public interface IApiHelper
    {
        /// <summary>
        /// просто пишет в response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task WriteResponseAsync<T>(HttpResponse response, T data);
        /// <summary>
        /// пытается приобразовать к return типу и пишет в response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task WriteReturnResponseAsync<T>(HttpResponse response, T data);
        string GetAccessTokenFromRequest(HttpRequest request);
        string GetRefreshTokenFromRequest(HttpRequest request);
        void ClearUsersTokens(HttpResponse response);
        UserInfo GetUserInfoFromRequest(HttpRequest request, IJWTService jwtService);
        (bool expired, UserInfo ui) GetUserInfoWithExpiredFromRequest(HttpRequest request, IJWTService jwtService);
        UserInfo CheckAuthorized(HttpRequest request, IJWTService jwtService, bool withError = false);
        (bool expired, UserInfo ui) GetUserInfoWithExpired(HttpRequest request, IJWTService jwtService, bool withError = false);

        Task DoStandartSomething(Func<Task> action, HttpResponse response, ILogger logger);

        void SetUserTokens(HttpResponse response, AllTokens tokens);
        void SetUserTokens(HttpResponse response, string accessToken, string refreshToken);

        void StopIfModelStateError(ModelStateDictionary modelState);


        object GetReturnType<TIn>(TIn obj);

        bool ErrorsFromModelState(ModelStateDictionary modelState);
        string StringValidator(string str);
        void FileValidator(IFormFile file, ModelStateDictionary modelState);
        Task SendFile(MemoryStream str, HttpResponse response,string fileName);
    }
}
