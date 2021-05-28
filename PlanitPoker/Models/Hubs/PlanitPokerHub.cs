using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.SignalR;
using PlanitPoker.Models.Repositories.Interfaces;

namespace PlanitPoker.Models.Hubs
{
    


    public class PlanitPokerHub : Hub
    {
        private readonly IPlanitPokerRepository _planitPokerRepository;
        private readonly MultiThreadHelper _multiThreadHelper;

        //front endpoints
        private   const string ConnectedToRoomError = "ConnectedToRoomError";
        private   const string NewRoomAlive = "NewRoomAlive";
        private const string NotifyFromServer = "NotifyFromServer";//todo будет принимать объект result с ошибками как в апи


        //TODO надо чистить то что приходит от юзера, уже реализовано просто прикрутить


        public PlanitPokerHub(IPlanitPokerRepository planitPokerRepository, MultiThreadHelper multiThreadHelper)
        {
            _planitPokerRepository = planitPokerRepository;
            _multiThreadHelper = multiThreadHelper;
        }
        //public async Task Send(string message)
        //{
        //    var userId = Context.UserIdentifier;
        //    //Groups.
        //    await this.Clients.All.SendAsync("Send", message);
        //}


        //login не нужен, просто будет храниться на фронте
        //public void Login(string username)
        //{
        //    if (string.IsNullOrWhiteSpace(username))
        //    {
        //        this.Clients.Caller.SendAsync();
        //    }

        //    if (Rooms.ContainsKey(roomname))
        //    {

        //    }
        //}

        //public void CreateRoom(string roomname)
        //{

        //}

        public async Task AliveRoom(string roomname)
        {
            var room = await _planitPokerRepository.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            await _planitPokerRepository.AddTimeAliveRoom(room);
            (var dt, bool suc) = _multiThreadHelper.GetProp(room, room => room.DieDate, room.RWL);
            if (suc)
            {
                await Clients.Group(roomname).SendAsync(NewRoomAlive, room.DieDate);
                return;
            }

            await Clients.Caller.SendAsync(NotifyFromServer,"retry plz");

        }

        public void EnterInRoom()
        {

        }

        public void KickUser()
        {

        }

        public void StartVote()
        {
            //чистим прошлые результаты
        }

        public void EndVote()
        {
            //запрещаем голосовать, подводим итоги
        }

        public override async Task OnConnectedAsync()
        {
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
