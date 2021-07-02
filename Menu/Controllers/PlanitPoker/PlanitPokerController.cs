using Common.Models.Error;
using Common.Models.Exceptions;
using Common.Models.Validators;
using WEB.Common.Models.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlanitPoker.Models.Services;
using System.Threading.Tasks;

namespace Menu.Controllers.PlanitPoker
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanitPokerController : ControllerBase
    {

        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IPlanitPokerService _planitPokerService;
        //private readonly IErrorService _errorService;
        //private readonly IHubContext<PlanitPokerHub> hubContext;


        private readonly IStringValidator _stringValidator;

        public PlanitPokerController(IApiHelper apiHealper,
            ILogger<PlanitPokerController> logger,
            IPlanitPokerService planitPokerService,
            IStringValidator stringValidator
            //IErrorService errorService
        )
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _planitPokerService = planitPokerService;
            _stringValidator = stringValidator;
            //_errorService = errorService;
            //hubContext.
        }


        [Route("get-users-in-room")]
        [HttpGet]
        public async Task GetUsersIsRoom(string roomname, string userConnectionId)
        {
            //todo подумать и мб перетащить это в хаб (в том числе из за потребности в userConnectionId)
            roomname = NormalizeRoomName(roomname);
            userConnectionId = _stringValidator.Validate(userConnectionId);
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var users = await _planitPokerService.GetAllUsersWithRight(roomname, userConnectionId);
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    await _apiHealper.WriteReturnResponseAsync(Response, users);

                }, Response, _logger);
        }


        [Route("get-room-info")]
        [HttpGet]
        public async Task GetRoomInfo(string roomname, string userConnectionId)
        {
            //todo подумать и мб перетащить это в хаб (в том числе из за потребности в userConnectionId)
            roomname = NormalizeRoomName(roomname);
            userConnectionId = _stringValidator.Validate(userConnectionId);
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var roomInfo =
                        await _planitPokerService.GetRoomInfoWithRight(roomname,
                            userConnectionId); //todo см declare метода в interface
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    if (roomInfo == null)
                    {
                        throw new SomeCustomException(ErrorConsts.SomeError);
                    }

                    await _apiHealper.WriteReturnResponseAsync(Response, roomInfo);

                }, Response, _logger);
        }




        private string NormalizeRoomName(string roomName)
        {
            return _stringValidator.Validate(roomName).ToUpper();
        }
    }
}
