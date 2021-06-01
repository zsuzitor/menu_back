﻿using Menu.Models.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PlanitPoker.Models.Hubs;
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
        //private readonly IHubContext<PlanitPokerHub> hubContext;

        public PlanitPokerController(IApiHelper apiHealper, 
            ILogger<PlanitPokerController> logger, 
            IPlanitPokerRepository planitPokerRepository)
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _planitPokerRepository = planitPokerRepository;
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
                   var users=await _planitPokerRepository.GetAllUsersWithRight(roomname, userid);
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
                    var roomInfo = await _planitPokerRepository.GetRoomInfo(roomname, userid);
                    //TODO ошибку если null? сейчас там возвращается пустая строка везде. и вообще посмотреть что будет на фронте
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
