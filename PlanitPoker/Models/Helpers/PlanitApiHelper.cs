﻿
using System;
using System.Threading.Tasks;
using Common.Models.Error;
using Common.Models.Error.Interfaces;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Returns;
using WEB.Common.Models.Helpers;
using WEB.Common.Models.Helpers.Interfaces;
using WEB.Common.Models.Returns.Interfaces;

namespace PlanitPoker.Models.Helpers
{
    public interface IPlanitApiHelper : IApiHelper
    {
        Task<bool> NotifyFromErrorService();
        void InitByHub(Hub hub);
        Task<T> DoStandartSomething<T>(Func<Task<T>> action, T defaultResult, HttpResponse response, ILogger logger);
    }

    public class PlanitApiHelper : ApiHelper, IPlanitApiHelper
    {
        private Hub _planitHub;

        public PlanitApiHelper(IErrorService errorService, IErrorContainer errorContainer, IReturnContainer returnContainer,
            IStringValidator stringValidator) : base(errorService, errorContainer, returnContainer, stringValidator)
        {
            _planitHub = null;
        }

        public void InitByHub(Hub hub)
        {
            _planitHub = hub;
        }

        public async Task<bool> NotifyFromErrorService()
        {
            if (_planitHub == null)
            {
                return false;
            }

            if (!_errorService.HasError())
            {
                return false;
            }

            
            var erF = new ErrorObjectReturnFactory();
            await _planitHub.Clients.Caller.SendAsync(Consts.PlanitPokerHubEndpoints.NotifyFromServer,
                erF.GetObjectReturn(_errorService.GetErrorsObject()));

            return true;
        }


        public override async Task DoStandartSomething(Func<Task> action, HttpResponse response, ILogger logger)
        {
            await DoStandartSomething(async () =>
            {
                await action();
                return true;
            }, false, response, logger);
            //try
            //{
            //    await action();
            //    return;
            //}
            //catch (SomeCustomException e)
            //{
            //    ErrorFromCustomException(e);
            //}
            //catch (StopException)
            //{
            //}
            //catch (NotAuthException)
            //{//ветка вообще не особо актуальная для покера
            //    var error = _errorContainer.TryGetError(ErrorConsts.NotAuthorized);
            //    if (error != null)
            //    {
            //        _errorService.AddError(error);
            //    }

            //    _errorService.AddError(_errorContainer.TryGetError(Consts.PlanitPokerErrorConsts.PlanitUserNotFound));
            //    //await WriteReturnResponseAsync(response, _errorService.GetErrorsObject(), 401);//TODO 401
            //    //return;
            //}
            //catch (Exception e)
            //{
            //    _errorService.AddError(_errorContainer.TryGetError(ErrorConsts.SomeError));
            //    logger?.LogError(e, ErrorConsts.SomeError);
            //}

            ////await WriteReturnResponseAsync(response, _errorService.GetErrorsObject());
            //await NotifyFromErrorService();
        }



        public async Task<T> DoStandartSomething<T>(Func<Task<T>> action, T defaultResult, HttpResponse response, ILogger logger)
        {
            try
            {
                return await action();

            }
            catch (SomeCustomException e)
            {
                ErrorFromCustomException(e);
            }
            catch (StopException)
            {
            }
            catch (NotAuthException)
            {//ветка вообще не особо актуальная для покера
                var error = _errorContainer.TryGetError(ErrorConsts.NotAuthorized);
                if (error != null)
                {
                    _errorService.AddError(error);
                }

                _errorService.AddError(_errorContainer.TryGetError(Consts.PlanitPokerErrorConsts.PlanitUserNotFound));
                //await WriteReturnResponseAsync(response, _errorService.GetErrorsObject(), 401);//TODO 401
                //return;
            }
            catch (Exception e)
            {
                _errorService.AddError(_errorContainer.TryGetError(ErrorConsts.SomeError));
                logger?.LogError(e, ErrorConsts.SomeError);
            }

            //await WriteReturnResponseAsync(response, _errorService.GetErrorsObject());
            await NotifyFromErrorService();
            return defaultResult;
        }
    }
}
