using Menu.Models.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlanitPoker.Models.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Menu.Controllers.PlanitPoker
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanitPokerController : ControllerBase
    {

        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IPlanitPokerRepository _planitPokerRepository;

        public PlanitPokerController(IApiHelper apiHealper, 
            ILogger<PlanitPokerController> logger, 
            IPlanitPokerRepository planitPokerRepository)
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _planitPokerRepository = planitPokerRepository;
        }


        [Route("get-users-in-room")]
        [HttpGet]
        public async Task GetUsersIsRoom(string roomname)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                   var users=await _planitPokerRepository.GetAllUsers(roomname);
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
                    await _apiHealper.WriteReturnResponseAsync(Response, users);

                }, Response, _logger);
        }

        [Route("create-room")]
        [HttpGet]
        public async Task CreateRoom( string roomname)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {


                    //await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("get-my-room")]
        [HttpGet]
        public async Task GetMyRoom( string connectionId)//можно достать на фронте
        {
            //todo наверное не нужно
        }
    }
}
