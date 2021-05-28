using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        private const string ConnectedToRoomError = "ConnectedToRoomError";
        private const string NewRoomAlive = "NewRoomAlive";
        private const string NotifyFromServer = "NotifyFromServer";//todo будет принимать объект result с ошибками как в апи
        private const string EnteredInRoom = "EnteredInRoom";
        private const string NewUserInRoom = "NewUserInRoom";


        //TODO надо чистить то что приходит от юзера, уже реализовано просто прикрутить
        //todo нужна кнопка "обновить список пользователей?"


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
            (var dt, bool suc) = _multiThreadHelper.GetValue(room, room => room.DieDate, room.RWL);
            if (suc)
            {
                await Clients.Group(roomname).SendAsync(NewRoomAlive, room.DieDate);
                return;
            }

            await Clients.Caller.SendAsync(NotifyFromServer, "retry plz");
        }

        public async Task EnterInRoom(string roomname, string password, string username)
        {
            if (string.IsNullOrEmpty(roomname))
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
            }

            var room = await _planitPokerRepository.TryGetRoom(roomname, password);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            var user = new PlanitUser()
            {
                UserIdentifier = Context.ConnectionId,
                Name = username,
                Role = GetDefaultRoles(),
            };

            var added = await _planitPokerRepository.AddUserIntoRoom(room, user);
            if (!added)
            {
                //TODO что то пошло не так
                await Clients.Caller.SendAsync(NotifyFromServer, "retry plz");
                return;
            }

            await Clients.Group(roomname).SendAsync(NewUserInRoom, room.DieDate);
            await Groups.AddToGroupAsync(user.UserIdentifier, roomname);

            (var usersInRoom, bool suc) = _multiThreadHelper.GetValue(room, room => room.Users.Select(x=>x.Clone()), room.RWL);
            if (suc)
            {
                //TODO отключить юзера и попросить переконнектиться
                return;
            }
            await Clients.Caller.SendAsync(EnteredInRoom, usersInRoom);

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

        private List<string> GetDefaultRoles()
        {
            return new List<string>() { "User" };
        }
    }
}
