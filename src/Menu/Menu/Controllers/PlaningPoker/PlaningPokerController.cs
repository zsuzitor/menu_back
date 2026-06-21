using Common.Models;
using Common.Models.Entity;
using Common.Models.Error;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Models.PlaningPoker.Requests;
using Menu.Models.PlaningPoker.Returns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlaningPoker.Models.Returns;
using PlaningPoker.Models.Services;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.PlaningPoker
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaningPokerController : ControllerBase
    {

        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IPlaningPokerService _planingPokerService;

        private readonly IJWTService _jwtService;

        private readonly IErrorService _errorService;



        private readonly PlaningUserReturnFactory _planingUserReturnFactory;

        public PlaningPokerController(IApiHelper apiHealper,
            ILoggerFactory loggerFactory,
            IPlaningPokerService planingPokerService, IJWTService jwtService,
        IErrorService errorService
        )
        {
            _apiHealper = apiHealper;
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);
            _planingPokerService = planingPokerService;
            _errorService = errorService;
            //hubContext.

            _jwtService = jwtService;
            _planingUserReturnFactory = new PlaningUserReturnFactory();
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
                    var users = await _planingPokerService.GetAllUsersWithRightAsync(roomname, userConnectionId);
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    await _apiHealper.WriteResponseAsync(Response, _planingUserReturnFactory.GetObjectReturn(users));

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
                        await _planingPokerService.GetRoomInfoWithRightAsync(roomname,
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
                    var stories = await _planingPokerService
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
                    await _planingPokerService.HandleInRoomsMemoryAsync();
                }, Response, _logger);
        }

        [Route("start-saving")]
        [HttpGet]
        public async Task StartSaving()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    await _planingPokerService.HandleInRoomsMemoryAsync(false, true);
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
                    var res = await _planingPokerService.ChangeRoomPasswordAsync(roomname, userConnectionId, oldPassword, newPassword);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultFactory().GetObjectReturn(new BoolResult(res)));
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

                    var rooms = await _planingPokerService
                        .GetRoomsAsync(userInfo.UserId);
                    var res = new { rooms = rooms.Select(x => new RoomShort(x)) };

                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }



        [Route("change-room-image")]
        [HttpPatch]
        public async Task ChangeRoomImage([FromForm] ChangeRoomImageRequest request)
        {
            request.RoomName = NormalizeRoomName(request.RoomName);
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  _apiHealper.FileValidator(request.Image, ModelState);
                  _apiHealper.ErrorsFromModelState(ModelState);
                  if (_errorService.HasError())
                  {
                      throw new StopException();
                  }

                  var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                  var res = await _planingPokerService.ChangeRoomImageAsync(request.RoomName, userInfo.UserId, request.Image);
                  if (request.Image != null && string.IsNullOrEmpty(res))
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