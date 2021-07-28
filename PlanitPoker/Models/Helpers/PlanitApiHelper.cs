
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
    }

    public class PlanitApiHelper:ApiHelper, IPlanitApiHelper
    {
        private Hub _planitHub;

        public PlanitApiHelper(IErrorService errorService, IErrorContainer errorContainer, IReturnContainer returnContainer,
            IStringValidator stringValidator):base(errorService, errorContainer, returnContainer, stringValidator)
        {
            _planitHub = null;
        }

        public void InitByHub(Hub hub)
        {
            _planitHub = hub;
        }

        public override async Task DoStandartSomething(Func<Task> action, HttpResponse response, ILogger logger)
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
            {//ветка вообще не особо актцально для покера
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

            await WriteReturnResponseAsync(response, _errorService.GetErrorsObject());
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

            await _planitHub.Clients.Caller.SendAsync(Consts.PlanitPokerHubEndpoints.NotifyFromServer,
                new Notify() { Text = "_errorService", Status = NotyfyStatus.Error });

            return true;
        }
    }
}
