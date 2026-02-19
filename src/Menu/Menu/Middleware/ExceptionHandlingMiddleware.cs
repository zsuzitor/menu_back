using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Return;
using BL.Models.Services.Interfaces;
using Common.Models.Error;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Common.Models;

namespace Menu.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        //protected readonly IErrorService errorService;
        //protected readonly IConfigurationService configurationService;
        private readonly RequestDelegate _next;


        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            //errorService = errorService;
            //configurationService = configurationService;
            _next = next;
        }

 
        //public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        public async Task InvokeAsync(HttpContext context, IErrorService errorService, IConfigurationService configurationService
            , ILoggerFactory loggerFactory)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, errorService, configurationService, loggerFactory.CreateLogger(Constants.Loggers.MenuApp));
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IErrorService errorService
            , IConfigurationService configurationService, ILogger logger)
        {

            switch(exception)
            {
                case SomeCustomNotAllowedException e:
                    context.Response.StatusCode = 403;
                    await ErrorFromCustomException(e, errorService, configurationService);
                    break;
                case SomeCustomNotFoundException e:
                    context.Response.StatusCode = 404;
                    await ErrorFromCustomException(e, errorService, configurationService);
                    break;
                case SomeCustomBadRequestException e:
                    context.Response.StatusCode = 400;
                    await ErrorFromCustomException(e, errorService, configurationService);
                    break;
                case SomeCustomException e:
                    context.Response.StatusCode = 406;
                    await ErrorFromCustomException(e, errorService, configurationService);
                    break;
                case StopException e:
                    break;
                case NotAuthException e:
                    {
                        context.Response.StatusCode = 401;
                        var error = await configurationService.GetAsync(ErrorConsts.NotAuthorized);
                        errorService.AddError(ErrorConsts.NotAuthorized, error.Value);
                        break;
                    }
                default :
                    {
                        context.Response.StatusCode = 500;
                        var error = await configurationService.GetAsync(ErrorConsts.SomeError);
                        errorService.AddError(ErrorConsts.SomeError, error.Value);
                        logger?.LogError(exception, ErrorConsts.SomeError);
                        break;
                    }
            }


            //return context.Response.WriteAsJsonAsync(response);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorObjectReturnFactory().GetObjectReturn(errorService.GetErrorsObject())));
        }


        private async Task ErrorFromCustomException(SomeCustomException e, IErrorService errorService, IConfigurationService configurationService)
        {
            if (string.IsNullOrWhiteSpace(e.Message))
            {
                return;
            }

            var error = await configurationService.GetAsync(e.Message);
            if (error != null)
            {
                errorService.AddError(e.Message, error.Value);
                if (!string.IsNullOrWhiteSpace(e.Body))
                {
                    errorService.AddError(e.Message, e.Body);
                }

                return;
            }

            if (!string.IsNullOrWhiteSpace(e.Body))
            {
                errorService.AddError(e.Message, e.Body);
            }
            else
            {
                errorService.AddError(ErrorConsts.SomeError, e.Message);
            }

        }
    }
}
