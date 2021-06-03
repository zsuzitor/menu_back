using Common.Models.Error;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Menu.Models.Helpers.Interfaces;
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

        public PlanitPokerController(IApiHelper apiHealper, 
            ILogger<PlanitPokerController> logger, 
            IPlanitPokerService planitPokerService//,
            //IErrorService errorService
            )
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _planitPokerService = planitPokerService;
            //_errorService = errorService;
            //hubContext.
        }


        [Route("get-users-in-room")]
        [HttpGet]
        public async Task GetUsersIsRoom(string roomname,string userid)
        {
            //TODO тк сейчас userId в открытом доступе
            //его лучше вот прям так не передавать
            //либо брать на бэке(перетаскивать логику в хаб)
            //либо закрывать id юзеров
            //а лучше и то и то
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                   var users=await _planitPokerService.GetAllUsersWithRight(roomname, userid);
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    await _apiHealper.WriteReturnResponseAsync(Response, users);

                }, Response, _logger);
        }


        [Route("get-room-info")]
        [HttpGet]
        public async Task GetRoomInfo(string roomname, string userid)
        {
            //TODO тк сейчас userId в открытом доступе
            //его лучше вот прям так не передавать
            //либо брать на бэке(перетаскивать логику в хаб)
            //либо закрывать id юзеров
            //а лучше и то и то
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var roomInfo = await _planitPokerService.GetRoomInfoWithRight(roomname, userid);
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    if (roomInfo == null)
                    {
                        //await _apiHealper.WriteReturnResponseAsync(Response, new Er);
                        throw new SomeCustomException(ErrorConsts.SomeError);
                    }
                    await _apiHealper.WriteReturnResponseAsync(Response, roomInfo);

                }, Response, _logger);
        }

        //[Route("create-room")]
        //[HttpGet]
        //public async Task CreateRoom( string roomname)
        //{
        //    await _apiHealper.DoStandartSomething(
        //        async () =>
        //        {


        //            //await _apiHealper.WriteReturnResponseAsync(Response, res);

        //        }, Response, _logger);

        //}

        //[Route("get-my-room")]
        //[HttpGet]
        //public async Task GetMyRoom( string connectionId)//можно достать на фронте
        //{
        //    //todo наверное не нужно
        //}
    }
}
