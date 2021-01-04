using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models.Poco;
using Menu.Models.Auth.Poco;
using Menu.Models.Error;
using Menu.Models.Error.Interfaces;
using Menu.Models.Error.services.Interfaces;
using Menu.Models.Exceptions;
using Menu.Models.Helpers.Interfaces;
using Menu.Models.Returns.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menu.Models.Helpers
{
    public class ApiHelper : IApiHelper
    {
        private readonly string _jsonContentType = "application/json";
        private readonly string _headerAccessToken = "Authorization_Access_Token";
        private readonly string _headerRefreshToken = "Authorization_Refresh_Token";


        private readonly IErrorService _errorService;
        private readonly IErrorContainer _errorContainer;

        /// <summary>
        /// только для моделей по умолчанию, те 1 тип маппится тут только с 1 return типом
        /// </summary>
        private readonly IReturnContainer _returnContainer;



        public ApiHelper(IErrorService errorService, IErrorContainer errorContainer, IReturnContainer returnContainer)
        {
            _errorService = errorService;
            _errorContainer = errorContainer;
            _returnContainer = returnContainer;
        }


        public async Task WriteResponseAsync<T>(HttpResponse response, T data)
        {
            response.ContentType = _jsonContentType;
            await response.WriteAsync(JsonSerializer.Serialize(data));
        }

        /// <summary>
        /// map return type with return_type_container and write response
        /// for write without mapping WriteResponseAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task WriteReturnResponseAsync<T>(HttpResponse response, T data)
        {
            response.ContentType = _jsonContentType;
            await response.WriteAsync(JsonSerializer.Serialize(GetReturnType(data)));
        }

        /// <summary>
        /// map return type with return_type_container and write response
        /// for write without mapping WriteResponseAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task WriteReturnResponseAsync<T>(HttpResponse response, T data, int status)
        {
            response.ContentType = _jsonContentType;
            response.StatusCode = status;
            await response.WriteAsync(JsonSerializer.Serialize(GetReturnType(data)));
        }


        /// <summary>
        /// return null if can not get UI 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="jwtService"></param>
        /// <returns></returns>
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

        public void SetUserTokens(HttpResponse response, AllTokens tokens)
        {
            SetUserTokens(response, tokens.AccessToken, tokens.RefreshToken);
        }

        public void SetUserTokens(HttpResponse response, string accessToken, string refreshToken)
        {
            response.Cookies.Append(_headerAccessToken, accessToken);
            response.Cookies.Append(_headerRefreshToken, refreshToken);
        }


        /// <summary>
        /// get from cookies or headers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetFromRequest(HttpRequest request, string key)
        {
            if (request.Cookies.TryGetValue(key, out var authorizationToken) && !string.IsNullOrWhiteSpace(authorizationToken))
            {
                return authorizationToken;
            }

            if (request.Headers.TryGetValue(key, out var authorizationTokenValue))
            {
                return authorizationTokenValue;
            }
            return null;

        }

        public UserInfo CheckAuthorized(HttpRequest request, IJWTService jwtService, bool withError = false)
        {
            var userInfo = GetUserInfoFromRequest(request, jwtService);
            if (userInfo == null || userInfo.UserId < 1)
            {
                //_errorService.AddError(_errorContainer.TryGetError("not_authorized"));
                if (withError)
                {
                    throw new NotAuthException();
                }

                return null;
            }

            return userInfo;
        }



        public async Task DoStandartSomething(Func<Task> action, HttpResponse response, ILogger logger)
        {
            try
            {
                await action();
                return;
            }
            catch (SomeCustomException e)
            {
                ErrorFromCustomException(e);
            }
            catch (StopException)
            {
            }
            catch (NotAuthException)
            {
                var error = _errorContainer.TryGetError(ErrorConsts.NotAuthorized);
                if (error != null)
                {
                    _errorService.AddError(error);
                }
                await WriteReturnResponseAsync(response, _errorService.GetErrorsObject(), 401);//TODO 401
                return;
            }
            catch (Exception e)
            {
                _errorService.AddError(_errorContainer.TryGetError(ErrorConsts.SomeError));
                logger?.LogError(e, ErrorConsts.SomeError);
            }

            await WriteReturnResponseAsync(response, _errorService.GetErrorsObject());
        }

        /// <summary>
        /// добавляет ошибку в errorService и все
        /// </summary>
        /// <param name="e"></param>
        private void ErrorFromCustomException(SomeCustomException e)
        {
            if (string.IsNullOrWhiteSpace(e.Message))
            {
                return;
            }

            var error = _errorContainer.TryGetError(e.Message);
            if (error != null)
            {
                if (!string.IsNullOrWhiteSpace(e.Body))
                {
                    error.Errors.Add(e.Body);
                }

                _errorService.AddError(error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(e.Body))
            {
                _errorService.AddError(e.Message, e.Body);
            }
            else
            {
                _errorService.AddError(ErrorConsts.SomeError, e.Message);
            }

        }


        public object GetReturnType<TIn>(TIn obj)
        {
            return _returnContainer.GetReturnType(obj);
        }

        public void StopIfModelStateError(ModelStateDictionary modelState)
        {
            if (_errorService.ErrorsFromModelState(modelState))
            {
                throw new StopException();
            }
        }
    }

}
