using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models.Poco;
using Common.Models.Error;
using Common.Models.Error.Interfaces;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using WEB.Common.Models.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using BO.Models.Auth;
//using WEB.Common.Models.Returns.Interfaces;
using System.IO;
using Common.Models.Return;
using Common.Models.Validators;
using WEB.Common.Models.Returns;

namespace WEB.Common.Models.Helpers
{
    public class ApiHelper : IApiHelper
    {
        //можно виртуал свойство enum например и по нему уже фабрику, но хз есть ли смысл
        public virtual IApiSerializer ApiSerializer { get; } = new JsonApiSerializer();

        //protected readonly string _jsonContentType = "application/json";
        protected readonly string _headerAccessToken = "Authorization_Access_Token";
        protected readonly string _headerRefreshToken = "Authorization_Refresh_Token";


        protected readonly IErrorService _errorService;
        protected readonly IErrorContainer _errorContainer;
        protected readonly long _fileMaxSize;

        /// <summary>
        /// только для моделей по умолчанию, те 1 тип маппится тут только с 1 return типом
        /// </summary>
        //protected readonly IReturnContainer _returnContainer;

        protected readonly IStringValidator _stringValidator;




        public ApiHelper(IErrorService errorService, IErrorContainer errorContainer,
            IStringValidator stringValidator)
        {
            _fileMaxSize = 1024 * 1024 * 3;

            _errorService = errorService;
            _errorContainer = errorContainer;
            //_returnContainer = returnContainer;
            _stringValidator = stringValidator;
        }


        public async Task WriteResponseAsync<T>(HttpResponse response, T data)
        {
            response.ContentType = ApiSerializer.ContentType;
            await response.WriteAsync(ApiSerializer.Serialize(data));
        }

        public async Task WriteResponseAsync<T>(HttpResponse response, T data, int status)
        {
            response.ContentType = ApiSerializer.ContentType;
            response.StatusCode = status;
            await response.WriteAsync(ApiSerializer.Serialize(data));
        }

        /// <summary>
        /// map return type with return_type_container and write response
        /// for write without mapping WriteResponseAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        //public async Task WriteReturnResponseAsync<T>(HttpResponse response, T data)
        //{
        //    response.ContentType = ApiSerializer.ContentType;

        //    await response.WriteAsync(ApiSerializer.Serialize(GetReturnType(data)));
        //}

        /// <summary>
        /// map return type with return_type_container and write response
        /// for write without mapping WriteResponseAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        //public async Task WriteReturnResponseAsync<T>(HttpResponse response, T data, int status)
        //{
        //    response.ContentType = ApiSerializer.ContentType;
        //    response.StatusCode = status;
        //    await response.WriteAsync(ApiSerializer.Serialize(GetReturnType(data)));
        //}


        /// <summary>
        /// exception or return null if can not get UI 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="jwtService"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoFromRequest(HttpRequest request, IJWTService jwtService)
        {
            var accessToken = GetAccessTokenFromRequest(request);
            //if (withExpired) {
            //    userId = jwtService.GetUserIdFromAccessTokenIfCan(accessToken);
            //        }
            //else
            //{
            string userId = jwtService.GetUserIdFromAccessToken(accessToken);

            //}
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

        public (bool expired, UserInfo ui) GetUserInfoWithExpiredFromRequest(HttpRequest request, IJWTService jwtService)
        {
            var accessToken = GetAccessTokenFromRequest(request);
            bool expired = false;
            string userId;
            try
            {
                userId = jwtService.GetUserIdFromAccessToken(accessToken);
            }
            catch
            {
                userId = jwtService.GetUserIdFromAccessTokenIfCan(accessToken);
                expired = true;
            }


            if (!long.TryParse(userId, out long userIdLong))
            {
                return (false, null);
            }

            var res = new UserInfo(userIdLong)
            {
                RefreshToken = GetRefreshTokenFromRequest(request),
                AccessToken = accessToken
            };

            return (expired, res);

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
            SetUserTokens(response, tokens?.AccessToken, tokens?.RefreshToken);
        }

        public void SetUserTokens(HttpResponse response, string accessToken, string refreshToken)
        {
            ClearUsersTokens(response);
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                response.Cookies.Append(_headerAccessToken, accessToken, new CookieOptions() { HttpOnly = true });
            }
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                response.Cookies.Append(_headerRefreshToken, refreshToken, new CookieOptions() { HttpOnly = true });
            }

        }


        /// <summary>
        /// get from cookies or headers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetFromRequest(HttpRequest request, string key)
        {
            if (request.Cookies.TryGetValue(key, out var authorizationToken)
                && !string.IsNullOrWhiteSpace(authorizationToken))
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
            try
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
            catch (Exception e)
            {
                if (withError)
                {
                    throw new NotAuthException("jwt_service_error", e);
                }
            }

            return null;
        }



        public (bool expired, UserInfo ui) GetUserInfoWithExpired(HttpRequest request, IJWTService jwtService, bool withError = false)
        {
            try
            {
                var (expired, userInfo) = GetUserInfoWithExpiredFromRequest(request, jwtService);
                if (userInfo == null || userInfo.UserId < 1)
                {
                    //_errorService.AddError(_errorContainer.TryGetError("not_authorized"));
                    if (withError)
                    {
                        throw new NotAuthException();
                    }

                    return (false, null);
                }

                return (expired, userInfo);
            }
            catch (Exception e)
            {
                if (withError)
                {
                    throw new NotAuthException("jwt_service_error", e);
                }
            }

            return (false, null);
        }


        //public async Task DoStandartSomethingWithOutErrorResponse(Func<Task> action, HttpResponse response, ILogger logger)
        //{

        //}

        public virtual async Task DoStandartSomething(Func<Task> action, HttpResponse response, ILogger logger)
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
                
                await WriteResponseAsync(response, new ErrorObjectReturnFactory().GetObjectReturn(_errorService.GetErrorsObject()), 401);//TODO 401
                return;
            }
            catch (Exception e)
            {
                _errorService.AddError(_errorContainer.TryGetError(ErrorConsts.SomeError));
                logger?.LogError(e, ErrorConsts.SomeError);
            }

            await WriteResponseAsync(response, new ErrorObjectReturnFactory().GetObjectReturn(_errorService.GetErrorsObject()));
        }

        /// <summary>
        /// добавляет ошибку в errorService и все
        /// </summary>
        /// <param name="e"></param>
        protected void ErrorFromCustomException(SomeCustomException e)
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


        //public object GetReturnType<TIn>(TIn obj)
        //{
        //    return _returnContainer.GetReturnType(obj);
        //}

        public void StopIfModelStateError(ModelStateDictionary modelState)
        {
            if (ErrorsFromModelState(modelState))
            {
                throw new StopException();
            }
        }

        //return true если есть ошибки
        public bool ErrorsFromModelState(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return false;
            }

            //var errors = modelState.ToList();
            //if (errors.Count == 0)
            //{
            //    return false;
            //}


            foreach (var keyInput in modelState.Keys)
            {
                var input = modelState[keyInput];

                if (input.Errors.Count == 0)
                {
                    continue;
                }

                var errObj = new OneError(keyInput, input.Errors.Select(x => x.ErrorMessage).ToList());
                _errorService.AddError(errObj);
            }

            return _errorService.HasError();
        }

        public string StringValidator(string str)
        {

            return _stringValidator.Validate(str) ;
        }

        public void FileValidator(IFormFile file, ModelStateDictionary modelState)
        {
            if (file == null)
            {
                return;
            }

            string fileContentType = file.ContentType.ToLower();
            if (fileContentType != "image/jpeg"
                && fileContentType != "image/jpg"
                && fileContentType != "image/png")
            {
                modelState.AddModelError(ErrorConsts.FileError, "Файл может быть jpeg, jpg, png");//TODO текст надо вынести
            }

            if (file.Length > _fileMaxSize)
            {
                modelState.AddModelError(ErrorConsts.FileError, "Максимальный размер файла 3мб");//TODO текст надо вынести
            }
        }

        public async Task SendFile(MemoryStream mr, HttpResponse response, string fileName)
        {
            mr.Position = 0;
            response.ContentType = "application/force-download";
            response.Headers.Add("content-disposition", $"inline;FileName=\"{fileName}\"");
            await response.Body.WriteAsync(mr.ToArray(), 0, (int)mr.Length);
        }
    }

}
