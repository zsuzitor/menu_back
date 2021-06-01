using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.SignalR;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Returns;

namespace PlanitPoker.Models.Hubs
{


    //https://github.com/SignalR/SignalR/wiki/SignalR-JS-Client#connectionid
    //https://metanit.com/sharp/aspnet5/30.3.php
    public class PlanitPokerHub : Hub
    {
        private readonly IPlanitPokerRepository _planitPokerRepository;
        private readonly MultiThreadHelper _multiThreadHelper;

        //private static IServiceProvider _serviceProvider;

        //front endpoints
        private const string ConnectedToRoomError = "ConnectedToRoomError";
        private const string NewRoomAlive = "NewRoomAlive";
        private const string NotifyFromServer = "NotifyFromServer";//todo будет принимать объект result с ошибками как в апи
        private const string EnteredInRoom = "EnteredInRoom";
        private const string NewUserInRoom = "NewUserInRoom";
        private const string UserLeaved = "UserLeaved";
        private const string VoteStart = "VoteStart";//голосование начато, оценки почищены
        private const string VoteEnd = "VoteEnd";
        //private const string VoteSuccess = "VoteSuccess";
        private const string VoteChanged = "VoteChanged";
        private const string RoomNotCreated = "RoomNotCreated";


        //todo now
        //начать голосование
        //закончить с подсчетом голосов мин макс среднее
        //при подключении к руме запрашивать всю инфу а не только статус
        //todo нужна кнопка "обновить список пользователей?"



        //TODO !!!!!!!!!!!надо потестить многопоточность, могу ли я без блокировки стучаться к room что бы получить lockobj или его надо выносить
        //настроить таймауты у сигналр + https://github.com/dotnet/aspnetcore/issues/20151

        //а безопасно ли показывать юзерам чужие id подключений?
        //TODO надо чистить то что приходит от юзера, уже реализовано просто прикрутить
        //todo очистка старых комнат
        //todo методы которые в Room надо вынести в репо, GetValueFromRoomAsync тоже
        //если 2 раза быстро нажать подключение к комнате, все норм отработает?



        public PlanitPokerHub(IPlanitPokerRepository planitPokerRepository, MultiThreadHelper multiThreadHelper)
        {
            _planitPokerRepository = planitPokerRepository;
            _multiThreadHelper = multiThreadHelper;
        }

        //static void InitStaticMembers()
        //{
        //    _serviceProvider = serviceProvider;
        //}
        //public async Task Send(string message)
        //{
        //    var userId = Context.UserIdentifier; --- так нельзя это что то дургое
        //    //Groups.
        //    await this.Clients.All.SendAsync("Send", message);
        //}

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }


        public async Task CreateRoom(string roomname, string password, string username)
        {
            var user = new PlanitUser()
            {
                UserIdentifier = Context.ConnectionId,
                Name = username,
                Role = GetCreatorRoles(),
            };

            var room = await _planitPokerRepository.CreateRoomWithUser(roomname, password, user);
            if (room == null)
            {
                await Clients.Caller.SendAsync(RoomNotCreated);
                return;
            }

            _ = await EnterInRoom(room, user);

        }


        public async Task AliveRoom(string roomname)
        {
            var room = await _planitPokerRepository.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            await _planitPokerRepository.AddTimeAliveRoom(room);
            (var dt, bool suc) = GetValueFromRoomAsync(room, room => room.StoredRoom.DieDate);
            if (suc)
            {
                await Clients.Group(roomname).SendAsync(NewRoomAlive, suc);
                return;
            }

            await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });
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

            _ = await EnterInRoom(room, user);



        }

        public async Task StartVote(string roomname)
        {
            var room = await _planitPokerRepository.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            _ = await _planitPokerRepository.ChangeStatus(room, Enums.RoomSatus.AllCanVote);
            _ = await _planitPokerRepository.ClearVotes(room);
            //await _multiThreadHelper.SetValue(room,async rm=> {
            //    await _planitPokerRepository.ChangeStatus(rm, Enums.RoomSatus.AllCanVote);

            //}, room.RWL);

            await Clients.Group(roomname).SendAsync(VoteStart);


        }

        public async Task EndVote(string roomname)
        {
            var room = await _planitPokerRepository.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            _ = await _planitPokerRepository.ChangeStatus(room, Enums.RoomSatus.AllCanVote);
            (var res, bool sc) = GetValueFromRoomAsync(room, rm =>
                  {
                      return rm.StoredRoom.Users.Select(x => x.Vote ?? 0);
                  });

            if (!sc)
            {
                //TODO
                return;
            }

            await Clients.Group(roomname).SendAsync(VoteEnd, res);
        }

        public async Task<bool> Vote(string roomname, int vote)
        {
            //вообще это вроде можно вынести в контроллер весь метод
            //тк сокеты не нужны для него, тупо апдейт стейта
            var room = await _planitPokerRepository.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return false;
            }

            (var res, bool sc) = GetValueFromRoomAsync(room, rm =>
            {
                return rm.StoredRoom.Status;
            });

            if (!sc)
            {
                //TODO
                return false;
            }

            if (res != RoomSatus.AllCanVote)
            {
                //todo можно написать что голосовать нельзя
                return false;
            }


            var changed  = await _planitPokerRepository.ChangeVote(room, Context.ConnectionId, vote);
            if (!changed)
            {
                return false;
            }

            var adminsId = await _planitPokerRepository.GetAdminsId(room);
            if(adminsId!=null&& adminsId.Count > 0)
            {
                await Clients.Clients(adminsId).SendAsync(VoteChanged, Context.ConnectionId, vote);
            }
            //await Clients.Caller.SendAsync(VoteSuccess, vote);
            return true;

        }

        public async Task KickUser(string roomname, string userId)
        {
            var kicked = await _planitPokerRepository.KickFromRoom(roomname, Context.ConnectionId, userId);

            if (kicked)
            {
                await Clients.Group(roomname).SendAsync(UserLeaved, userId);
                return;
            }

            await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });


        }



        public override async Task OnConnectedAsync()
        {
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            //TODO надо передать админку
            //название комнаты смогу вытащить из кук???
            await base.OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// рума уже создана и добавлена, пользователь еще нет
        /// </summary>
        /// <param name="room"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<bool> EnterInRoom(Room room, PlanitUser user)
        {
            if (room == null || user == null)
            {
                return false;
            }

            var username = user.Name;
            var userId = user.UserIdentifier;

            var added = await _planitPokerRepository.AddUserIntoRoom(room, user);
            if (!added)
            {
                //TODO что то пошло не так
                await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });

                return false;
            }

            var roomnm = room.GetConcurentValue(_multiThreadHelper, x => x.StoredRoom.Name);
            if (!roomnm.sc)
            {
                //TODO что то пошло не так
                await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });

                return false;
            }

            //специально до добавление юзера тк ему это сообщение не нужно
            var us = GetValueFromRoomAsync(room,(rm)=>rm.StoredRoom.Users.FirstOrDefault(x=>x.UserIdentifier==userId));
            PlanitUserReturn returnUser = null;
            if (us.sc)
            {
                returnUser = new PlanitUserReturn(us.res);
            }
            await Clients.Group(roomnm.res).SendAsync(NewUserInRoom, returnUser);
            await Groups.AddToGroupAsync(userId, roomnm.res);

            //(var usersInRoom, bool suc) = GetValueFromRoomAsync(room, room => room.StoredRoom.Users.Select(x => x.Clone()));
            //if (!suc)
            //{
            //    //TODO отключить юзера и попросить переконнектиться
            //    return false;
            //}
            await Clients.Caller.SendAsync(EnteredInRoom); //,usersInRoom//todo мб лучше отдельным запросом?
            return true;
        }


        private List<string> GetDefaultRoles()
        {
            return new List<string>() { "User" };
        }

        private List<string> GetCreatorRoles()
        {
            return new List<string>(GetDefaultRoles()) { "Creator", "Admin" };//TODO это надо вынести в константы, в том числе в return фарике в основной апе menu
        }

        //private static Task ClearRooms()
        //{

        //}

        private (T res, bool sc) GetValueFromRoomAsync<T>(Room room, Func<Room, T> get)
        {//TODO перетащить в репо
            return _multiThreadHelper.GetValue(room, get, room.RWL);
        }
    }
}
