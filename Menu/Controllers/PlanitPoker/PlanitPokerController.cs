using Common.Models.Error;
using Common.Models.Exceptions;
using Common.Models.Validators;
using WEB.Common.Models.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlanitPoker.Models.Services;
using System.Threading.Tasks;
using Menu.Models.Returns.Types.PlanitPoker;
using System.Linq;
using PlanitPoker.Models.Returns;
using Common.Models.Poco;
using Common.Models.Return;

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
        //private readonly IHubContext<PlanitPokerHub> _hubContext;


        private readonly IStringValidator _stringValidator;


        private readonly PlanitUserReturnFactory _planitUserReturnFactory;

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

            _planitUserReturnFactory = new PlanitUserReturnFactory();
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
                    var users = await _planitPokerService.GetAllUsersWithRightAsync(roomname, userConnectionId);
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    await _apiHealper.WriteResponseAsync(Response, _planitUserReturnFactory.GetObjectReturn(users));

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
                        await _planitPokerService.GetRoomInfoWithRightAsync(roomname,
                            userConnectionId); //todo см declare метода в interface
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    if (roomInfo == null)
                    {
                        throw new SomeCustomException(ErrorConsts.SomeError);
                    }

                    await _apiHealper.WriteResponseAsync(Response, roomInfo);

                }, Response, _logger);
        }


        [Route("get-not-actual-stories")]
        [HttpGet]
        public async Task GetNotActualStories(string roomname, string userConnectionId
            , int pageNumber, int pageSize)
        {
            //todo подумать и мб перетащить это в хаб (в том числе из за потребности в userConnectionId)
            roomname = NormalizeRoomName(roomname);
            userConnectionId = _stringValidator.Validate(userConnectionId);
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var stories = await _planitPokerService
                        .GetNotActualStoriesAsync(roomname, userConnectionId, pageNumber, pageSize);
                    var res = new { stories = stories.Select(x => new StoryReturn(x)) };

                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }



        [Route("start-clearing")]
        [HttpGet]
        public async Task StartClearing()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    await _planitPokerService.ClearOldRoomsAsync();
                }, Response, _logger);
        }


        [Route("room-password-change")]
        [HttpPatch]
        public async Task RoomPasswordChange([FromForm] string roomname
            , [FromForm] string userConnectionId, [FromForm] string oldPassword
            , [FromForm] string newPassword)
        {
            roomname = NormalizeRoomName(roomname);
            userConnectionId = _stringValidator.Validate(userConnectionId);
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var res = await _planitPokerService.ChangeRoomPasswordAsync(roomname, userConnectionId, oldPassword, newPassword);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultFactory().GetObjectReturn(new BoolResult(res)));
                    //await _planitPokerService.ClearOldRoomsAsync();
                }, Response, _logger);
        }


       



        private string NormalizeRoomName(string roomName)
        {
            return _stringValidator.Validate(roomName)?.ToUpper();
        }
    }
}