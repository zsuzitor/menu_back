using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Validators;
using Microsoft.AspNetCore.SignalR;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Returns;
using PlanitPoker.Models.Services;

namespace PlanitPoker.Models.Hubs
{


    //https://github.com/SignalR/SignalR/wiki/SignalR-JS-Client#connectionid
    //https://metanit.com/sharp/aspnet5/30.3.php
    public class PlanitPokerHub : Hub
    {
        private readonly IPlanitPokerRepository _planitPokerRepository;
        private readonly MultiThreadHelper _multiThreadHelper;
        private readonly IStringValidator _stringValidator;
        private readonly IPlanitPokerService _planitPokerService;

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
        private const string UserNameChanged = "UserNameChanged";
        private const string UserStatusChanged = "UserStatusChanged";
        private const string AddedNewStory = "AddedNewStory";
        private const string CurrentStoryChanged = "CurrentStoryChanged";
        private const string NewCurrentStory = "NewCurrentStory";
        private const string DeletedStory = "DeletedStory";
        private const string MovedStoryToComplete = "MovedStoryToComplete";







        public PlanitPokerHub(IPlanitPokerRepository planitPokerRepository, MultiThreadHelper multiThreadHelper,
            IStringValidator stringValidator, IPlanitPokerService planitPokerService)
        {
            _planitPokerRepository = planitPokerRepository;
            _multiThreadHelper = multiThreadHelper;
            _stringValidator = stringValidator;
            _planitPokerService = planitPokerService;
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
            roomname = ValidateString(roomname);
            username = ValidateString(username);

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
            roomname = ValidateString(roomname);
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
            roomname = ValidateString(roomname);
            username = ValidateString(username);
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
        {//TODO слишком много запросов, надо вынести в 1 и засунуть его в сервис лучше
            roomname = ValidateString(roomname);
            var room = await _planitPokerRepository.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            var success = await _planitPokerRepository.ChangeStatusIfCan(room, Context.ConnectionId, Enums.RoomSatus.AllCanVote);
            if (!success)
            {
                return;
            }
            success = await _planitPokerRepository.ClearVotes(room);
            if (!success)
            {
                return;
            }
            //await _multiThreadHelper.SetValue(room,async rm=> {
            //    await _planitPokerRepository.ChangeStatus(rm, Enums.RoomSatus.AllCanVote);

            //}, room.RWL);

            await Clients.Group(roomname).SendAsync(VoteStart);


        }

        public async Task EndVote(string roomname)
        {
            roomname = ValidateString(roomname);
            var room = await _planitPokerRepository.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            var success = await _planitPokerRepository.ChangeStatusIfCan(room, Context.ConnectionId, Enums.RoomSatus.CloseVote);
            if (!success)
            {
                return;
            }

            (var res, bool sc) = GetValueFromRoomAsync(room, rm =>
                  {
                      return rm.StoredRoom.Users.Select(x => new { userId = x.UserIdentifier, vote = x.Vote ?? 0 });
                  });

            if (!sc)
            {
                //TODO
                return;
            }


            var result = new EndVoteInfo();
            result.MinVote = res.Min(x => x.vote);
            result.MaxVote = res.Max(x => x.vote);
            result.Average = res.Average(x => x.vote);
            result.UsersInfo = res.Select(x => new EndVoteUserInfo() { Id = x.userId, Vote = x.vote }).ToList();

            await Clients.Group(roomname).SendAsync(VoteEnd, result);
        }

        public async Task<bool> Vote(string roomname, int vote)
        {

            roomname = ValidateString(roomname);

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


            var changed = await _planitPokerRepository.ChangeVote(room, Context.ConnectionId, vote);
            if (!changed)
            {
                return false;
            }

            var adminsId = await _planitPokerRepository.GetAdminsId(room);
            if (adminsId != null && adminsId.Count > 0)
            {
                await Clients.Clients(adminsId).SendAsync(VoteChanged, Context.ConnectionId, vote);
            }

            await Clients.GroupExcept(roomname, adminsId).SendAsync(VoteChanged, Context.ConnectionId, "?");
            //await Clients.Caller.SendAsync(VoteSuccess, vote);
            return true;

        }

        public async Task KickUser(string roomname, string userId)
        {
            roomname = ValidateString(roomname);
            userId = ValidateString(userId);
            var kicked = await _planitPokerRepository.KickFromRoom(roomname, Context.ConnectionId, userId);

            if (kicked)
            {
                await Clients.Group(roomname).SendAsync(UserLeaved, userId);
                return;
            }

            await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });


        }


        public async Task<bool> UserNameChange(string roomname, string newUserName)
        {
            roomname = ValidateString(roomname);
            newUserName = ValidateString(newUserName);
            string userId = Context.ConnectionId;
            var sc = await _planitPokerRepository.ChangeUserName(roomname, userId, newUserName);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(UserNameChanged, userId, newUserName);
                return true;
            }

            return false;

        }

        public async Task AddNewStatusToUser(string roomname, string userId, string newRole)
        {

            roomname = ValidateString(roomname);
            userId = ValidateString(userId);
            newRole = ValidateString(newRole);

            var sc = await _planitPokerRepository.AddNewStatusToUser(roomname, userId, newRole, Context.ConnectionId);
            if (sc)
            {
                await Clients.Group(roomname).SendAsync(UserStatusChanged, userId, 1, newRole);
            }
        }

        public async Task RemoveStatusUser(string roomname, string userId, string oldRole)
        {
            roomname = ValidateString(roomname);
            userId = ValidateString(userId);
            oldRole = ValidateString(oldRole);

            var sc = await _planitPokerRepository.RemoveStatusUser(roomname, userId, oldRole, Context.ConnectionId);
            if (sc)
            {
                await Clients.Group(roomname).SendAsync(UserStatusChanged, userId, 2, oldRole);
            }
        }


        public async Task AddNewStory(string roomname, string storyName, string storyDescription)
        {
            roomname = ValidateString(roomname);
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            //invoke AddedNewStory
            var newStory = new Story()
            {
                Name = storyName,
                Description = storyDescription,
            };

            var sc = await _planitPokerRepository.AddNewStory(roomname,Context.ConnectionId, newStory);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(AddedNewStory, new StoryReturn(newStory));
            }
        }


        public async Task ChangeCurrentStory(string roomname,long storyId, string storyName, string storyDescription)
        {
            roomname = ValidateString(roomname);
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            var newStory = new Story()
            {
                Id = storyId,
                Name = storyName,
                Description = storyDescription,
            };

            var sc = await _planitPokerRepository.ChangeStory(roomname, Context.ConnectionId, newStory);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(CurrentStoryChanged,storyId,storyName,storyDescription );//new StoryReturn(newStory)
            }
        }

        public async Task MakeCurrentStory(string roomname, long storyId)
        {
            roomname = ValidateString(roomname);
            //todo NewCurrentStory
            var sc = await _planitPokerRepository.ChangeCurrentStory(roomname, Context.ConnectionId, storyId);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(NewCurrentStory, storyId);
            }
        }

        public async Task DeleteStory(string roomname, long storyId)
        {
            roomname = ValidateString(roomname);
            //todo DeletedStory
            var sc = await _planitPokerRepository.DeleteStory(roomname, Context.ConnectionId, storyId);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(DeletedStory, storyId);
            }
        }


        public async Task MakeStoryComplete(string roomname, long storyId)
        {
            var res = await _planitPokerRepository.MakeStoryComplete(roomname, storyId, Context.ConnectionId);
            if (res)
            {
                await Clients.Group(roomname).SendAsync(MovedStoryToComplete, storyId);
            }    
            //
        }




        //public async Task SetCurrentStory(string roomname, string storyId)
        //{
        //}

        //public async Task RemoveStory(string roomname, string storyId)
        //{
        //}



        //------------------------------------ signal----------------------------------------------



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






        //----------------------------------------------------------------------------------private------------------


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
            var us = GetValueFromRoomAsync(room, (rm) => rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == userId));
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
            return new List<string>() { Consts.Roles.User };
        }

        private List<string> GetCreatorRoles()
        {
            return new List<string>(GetDefaultRoles()) { Consts.Roles.Creator, Consts.Roles.Admin };
        }

        //private static Task ClearRooms()
        //{

        //}

        private (T res, bool sc) GetValueFromRoomAsync<T>(Room room, Func<Room, T> get)
        {
            return room.GetConcurentValue(_multiThreadHelper, get);
            //return _multiThreadHelper.GetValue(room, get, room.RWL);
        }

        private string ValidateString(string str)
        {
            return _stringValidator.Validate(str);
        }
    }
}
