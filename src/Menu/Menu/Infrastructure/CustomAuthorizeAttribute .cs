
using Common.Models;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB.Common.Models.Helpers.Interfaces;


namespace Menu.Host.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;
        private readonly bool _withError;

        public CustomAuthorizeAttribute(bool withError = true)
        {
            _roles = Array.Empty<string>();
            _withError = withError;
        }

        public CustomAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 1. Пропускаем, если есть анонимный доступ
            if (context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
            {
                return;
            }

            //var token = context.HttpContext.Request.Headers["Authorization"]
            //    .FirstOrDefault()?.Replace("Bearer ", "");

            //if (string.IsNullOrEmpty(token))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            var apiHelper = context.HttpContext.RequestServices
                .GetRequiredService<IApiHelper>();
            var jwtService = context.HttpContext.RequestServices
                .GetRequiredService<IJWTService>();

            var userinfo = apiHelper.CheckAuthorized(context.HttpContext.Request, jwtService, _withError);

            //if (!validationResult.IsValid)
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            //if (_roles.Any() && !_roles.Any(r => validationResult.Roles.Contains(r)))
            //{
            //    context.Result = new ForbidResult(); // 403 Forbidden
            //    return;
            //}

            // 6. Сохраняем информацию о пользователе в HttpContext
            var claims = new List<System.Security.Claims.Claim>
        {
            new System.Security.Claims.Claim(
                Constants.Claims.Id,
                userinfo.UserId.ToString()),
        };

            //foreach (var role in userinfo.Roles)
            //{
            //    claims.Add(new System.Security.Claims.Claim(
            //        System.Security.Claims.ClaimTypes.Role, role));
            //}

            var identity = new System.Security.Claims.ClaimsIdentity(
                claims, "Custom");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);

            context.HttpContext.User = principal;
        }
    }
}
