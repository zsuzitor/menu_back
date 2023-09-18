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
using Common.Models.Entity;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Http;
using Common.Models.Error.services.Interfaces;

namespace Menu.Controllers.PlanitPoker
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanitPokerController : ControllerBase
    {

        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IPlanitPokerService _planitPokerService;

        private readonly IJWTService _jwtService;

        private readonly IErrorService _errorService;
        //private readonly IHubContext<PlanitPokerHub> _hubContext;



        private readonly PlanitUserReturnFactory _planitUserReturnFactory;

        public PlanitPokerController(IApiHelper apiHealper,
            ILogger<PlanitPokerController> logger,
            IPlanitPokerService planitPokerService, IJWTService jwtService,
        IErrorService errorService
        )
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _planitPokerService = planitPokerService;
            _errorService = errorService;
            //hubContext.

            _jwtService = jwtService;
            _planitUserReturnFactory = new PlanitUserReturnFactory();
        }


        [Route("get-users-in-room")]
        [HttpGet]
        public async Task GetUsersIsRoom(string roomname, string userConnectionId)
        {
            //todo подумать и мб перетащить это в хаб (в том числе из за потребности в userConnectionId)
            roomname = NormalizeRoomName(roomname);
            userConnectionId = _apiHealper.StringValidator(userConnectionId);
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
            userConnectionId = _apiHealper.StringValidator(userConnectionId);
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
            userConnectionId = _apiHealper.StringValidator(userConnectionId);
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
                    await _planitPokerService.HandleInRoomsMemoryAsync();
                }, Response, _logger);
        }

        [Route("start-saving")]
        [HttpGet]
        public async Task StartSaving()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    await _planitPokerService.HandleInRoomsMemoryAsync(false, true);
                }, Response, _logger);
        }

        [Route("room-password-change")]
        [HttpPatch]
        public async Task RoomPasswordChange([FromForm] string roomname
            , [FromForm] string userConnectionId, [FromForm] string oldPassword
            , [FromForm] string newPassword)
        {
            roomname = NormalizeRoomName(roomname);
            userConnectionId = _apiHealper.StringValidator(userConnectionId);
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var res = await _planitPokerService.ChangeRoomPasswordAsync(roomname, userConnectionId, oldPassword, newPassword);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultFactory().GetObjectReturn(new BoolResult(res)));
                    //await _planitPokerService.ClearOldRoomsAsync();
                }, Response, _logger);
        }


        [Route("get-my-rooms")]
        [HttpGet]
        public async Task GetRooms()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var rooms = await _planitPokerService
                        .GetRoomsAsync(userInfo.UserId);
                    var res = new { rooms = rooms.Select(x => new RoomShort(x)) };

                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }



        [Route("change-room-image")]
        [HttpPatch]
        public async Task ChangeRoomImage([FromForm] string roomname, [FromForm] IFormFile image)
        {
            roomname = NormalizeRoomName(roomname);
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  _apiHealper.FileValidator(image, ModelState);
                  _apiHealper.ErrorsFromModelState(ModelState);
                  if (_errorService.HasError())
                  {
                      throw new StopException();
                  }

                  var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                  var res = await _planitPokerService.ChangeRoomImageAsync(roomname, userInfo.UserId, image);
                  if (image != null && string.IsNullOrEmpty(res))
                  {
                      throw new SomeCustomException(ErrorConsts.SomeError);
                  }

                  await _apiHealper.WriteResponseAsync(Response, new StringResultReturn(res));

              }, Response, _logger);

        }


        private string NormalizeRoomName(string roomName)
        {
            return _apiHealper.StringValidator(roomName)?.ToUpper();
        }
    }
}