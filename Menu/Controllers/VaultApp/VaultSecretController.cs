using Common.Models.Error.services.Interfaces;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Returns.Types.VaultApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using VaultApp.Models.Entity.Input;
using VaultApp.Models.Services;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.VaultApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaultSecretController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IJWTService _jwtService;
        private readonly IErrorService _errorService;

        private readonly ISecretService _secretService;


        public VaultSecretController(IApiHelper apiHealper,
            ILogger<VaultController> logger, IJWTService jwtService,
        IErrorService errorService, ISecretService secretService
        )
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _errorService = errorService;
            _jwtService = jwtService;
            _secretService = secretService;
        }

        [Route("get-vault-secrets")]
        [HttpGet]
        public async Task GetVaultSecrets(long vaultId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = (await _secretService.GetSecretsAsync(vaultId, userInfo))
                        .Select(x => new SecretReturn().Fill(x));
                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }

        [Route("delete-secret")]
        [HttpDelete]
        public async Task DeleteSecret([FromForm] long secretId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _secretService.DeleteSecretAsync(secretId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);
        }

        [Route("create-secret")]
        [HttpPut]
        public async Task CreateSecret([FromForm] CreateSecret secret)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _secretService.CreateSecretAsync(secret, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new SecretReturn().Fill(res));

                }, Response, _logger);
        }


        [Route("update-secret")]
        [HttpPatch]
        public async Task UpdateSecret([FromForm] UpdateSecret secret)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _secretService.UpdateSecretAsync(secret, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new SecretReturn().Fill(res));

                }, Response, _logger);
        }

        [Route("get-secret")]
        [HttpGet]
        public async Task GetSecret(long secretId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _secretService.GetSecretAsync(secretId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new SecretReturn().Fill(res));

                }, Response, _logger);
        }
    }
}
