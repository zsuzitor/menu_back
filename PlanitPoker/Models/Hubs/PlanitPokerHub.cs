﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BO.Models.Auth;
using Common.Models;
using Common.Models.Validators;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.SignalR;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Returns;
using PlanitPoker.Models.Services;
using WEB.Common.Models.Helpers.Interfaces;

namespace PlanitPoker.Models.Hubs
{


    //https://github.com/SignalR/SignalR/wiki/SignalR-JS-Client#connectionid
    //https://metanit.com/sharp/aspnet5/30.3.php
    public class PlanitPokerHub : Hub
    {
        
        private readonly MultiThreadHelper _multiThreadHelper;
        private readonly IStringValidator _stringValidator;
        private readonly IPlanitPokerService _planitPokerService;
        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly IJWTHasher _hasher;

     


        //private static IServiceProvider _serviceProvider;

        //front endpoints
        private const string ConnectedToRoomError = "ConnectedToRoomError";
        private const string NewRoomAlive = "NewRoomAlive";
        private const string NotifyFromServer = "PlaningNotifyFromServer";//todo будет принимать объект result с ошибками как в апи
        private const string EnteredInRoom = "EnteredInRoom";
        private const string NewUserInRoom = "NewUserInRoom";
        private const string UserLeaved = "UserLeaved";
        private const string VoteStart = "VoteStart";//голосование начато, оценки почищены
        private const string VoteEnd = "VoteEnd";
        //private const string VoteSuccess = "VoteSuccess";
        private const string VoteChanged = "VoteChanged";
        private const string RoomNotCreated = "RoomNotCreated";
        private const string UserNameChanged = "UserNameChanged";
        private const string UserRoleChanged = "UserRoleChanged";
        private const string AddedNewStory = "AddedNewStory";
        private const string CurrentStoryChanged = "CurrentStoryChanged";
        private const string NewCurrentStory = "NewCurrentStory";
        private const string DeletedStory = "DeletedStory";
        private const string MovedStoryToComplete = "MovedStoryToComplete";
        private const string NeedRefreshTokens = "NeedRefreshTokens";







        public PlanitPokerHub(IPlanitPokerRepository planitPokerRepository, MultiThreadHelper multiThreadHelper,
            IStringValidator stringValidator, IPlanitPokerService planitPokerService,
            IApiHelper apiHealper, IJWTService jwtService, IJWTHasher hasher)
        {
            _multiThreadHelper = multiThreadHelper;
            _stringValidator = stringValidator;
            _planitPokerService = planitPokerService;
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _hasher = hasher;

            
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
            roomname = ValidateString(roomname).ToUpper();
            username = ValidateString(username);
            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }
            else
            {
                password = _hasher.GetHash(password);
            }

            UserInfo userInfo = null;
            var expired = false;
            try
            {
                (expired, userInfo) = _apiHealper.GetUserInfoWithExpired(Context.GetHttpContext().Request, _jwtService, false);
            }
            catch
            { }

            if (expired && userInfo != null)
            {
                //не прерываем процесс создания румы, но сообщаем о том что нужен рефреш
                await Clients.Caller.SendAsync(NeedRefreshTokens);
            }

            var user = new PlanitUser()
            {
                MainAppUserId = userInfo?.UserId,
                PlaningAppUserId = GenerateUniqueUserId(),
                UserConnectionId = GetConnectionId(),
                Name = username,
                Role = GetCreatorRoles(),
            };

            var room = await _planitPokerService.CreateRoomWithUser(roomname, password, user);
            if (room == null)
            {
                await Clients.Caller.SendAsync(RoomNotCreated);
                return;
            }

            _ = await EnterInRoom(room, user);

        }


        public async Task AliveRoom(string roomname)
        {
            roomname = ValidateString(roomname).ToUpper();
            var room = await _planitPokerService.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            await _planitPokerService.AddTimeAliveRoom(room);
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
            roomname = ValidateString(roomname).ToUpper();
            username = ValidateString(username);
            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }
            else
            {
                password = _hasher.GetHash(password);
            }


            if (string.IsNullOrEmpty(roomname))
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
            }

            var room = await _planitPokerService.TryGetRoom(roomname, password);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            UserInfo userInfo = null;
            var expired = false;
            try
            {
                (expired, userInfo) = _apiHealper.GetUserInfoWithExpired(Context.GetHttpContext().Request, _jwtService, false);
            }
            catch
            { }

            if (expired && userInfo != null)
            {
                //не прерываем процесс подключения, но сообщаем о том что нужен рефреш
                await Clients.Caller.SendAsync(NeedRefreshTokens);
            }

            var user = new PlanitUser()
            {
                MainAppUserId = userInfo?.UserId,
                PlaningAppUserId = GenerateUniqueUserId(),
                UserConnectionId = GetConnectionId(),
                Name = username,
                Role = GetDefaultRoles(),
            };

            _ = await EnterInRoom(room, user);

        }

        public async Task StartVote(string roomname)
        {//TODO слишком много запросов, надо вынести в 1 и засунуть его в сервис лучше
            roomname = ValidateString(roomname).ToUpper();
            var room = await _planitPokerService.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            var success = await _planitPokerService.ChangeStatusIfCan(room, GetConnectionId(), Enums.RoomSatus.AllCanVote);
            if (!success)
            {
                return;
            }
            success = await _planitPokerService.ClearVotes(room);
            if (!success)
            {
                return;
            }
            //await _multiThreadHelper.SetValue(room,async rm=> {
            //    await _planitPokerService.ChangeStatus(rm, Enums.RoomSatus.AllCanVote);

            //}, room.RWL);

            await Clients.Group(roomname).SendAsync(VoteStart);


        }

        public async Task EndVote(string roomname)
        {
            roomname = ValidateString(roomname).ToUpper();
            var room = await _planitPokerService.TryGetRoom(roomname);
            if (room == null)
            {
                await Clients.Caller.SendAsync(ConnectedToRoomError);
                return;
            }

            var success = await _planitPokerService.ChangeStatusIfCan(room, GetConnectionId(), Enums.RoomSatus.CloseVote);
            if (!success)
            {
                return;
            }

            var result = await _planitPokerService.GetEndVoteInfo(room);
            if (result == null)
            {
                //todo
                return;
            }
            //(var res, bool sc) = GetValueFromRoomAsync(room, rm =>
            //      {
            //          return rm.StoredRoom.Users.Select(x => new { userId = x.PlaningAppUserId, vote = x.Vote ?? 0 });
            //      });

            //if (!sc)
            //{
            //    //TODO
            //    return;
            //}


            //var result = new EndVoteInfo();
            //result.MinVote = res.Min(x => x.vote);
            //result.MaxVote = res.Max(x => x.vote);
            //result.Average = res.Average(x => x.vote);
            //result.UsersInfo = res.Select(x => new EndVoteUserInfo() { Id = x.userId, Vote = x.vote }).ToList();

            await Clients.Group(roomname).SendAsync(VoteEnd, result);
        }

        public async Task<bool> Vote(string roomname, int vote)
        {

            roomname = ValidateString(roomname).ToUpper();

            var room = await _planitPokerService.TryGetRoom(roomname);
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


            (bool sc1, string userId) = await _planitPokerService.ChangeVote(room, GetConnectionId(), vote);
            if (!sc1)
            {
                return false;
            }

            var adminsId = await _planitPokerService.GetAdminsId(room);
            if (adminsId != null && adminsId.Count > 0)
            {
                await Clients.Clients(adminsId).SendAsync(VoteChanged, userId, vote);
            }

            await Clients.GroupExcept(roomname, adminsId).SendAsync(VoteChanged, userId, "?");
            //await Clients.Caller.SendAsync(VoteSuccess, vote);
            return true;

        }

        public async Task KickUser(string roomname, string userId)
        {
            roomname = ValidateString(roomname).ToUpper();
            userId = ValidateString(userId);
            var kicked = await _planitPokerService.KickFromRoom(roomname, GetConnectionId(), userId);

            if (kicked)
            {
                await Clients.Group(roomname).SendAsync(UserLeaved, new List<string>() { userId });
                return;
            }

            await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });


        }


        public async Task<bool> UserNameChange(string roomname, string newUserName)
        {
            roomname = ValidateString(roomname).ToUpper();
            newUserName = ValidateString(newUserName);
            string userConnectionId = GetConnectionId();
            (bool sc, string userId) = await _planitPokerService.ChangeUserName(roomname, userConnectionId, newUserName);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(UserNameChanged, userId, newUserName);
                return true;
            }

            return false;

        }

        public async Task AddNewRoleToUser(string roomname, string userId, string newRole)
        {

            roomname = ValidateString(roomname).ToUpper();
            userId = ValidateString(userId);
            newRole = ValidateString(newRole);

            var sc = await _planitPokerService.AddNewRoleToUser(roomname, userId, newRole, GetConnectionId());
            if (sc)
            {
                await Clients.Group(roomname).SendAsync(UserRoleChanged, userId, 1, newRole);
            }
        }

        public async Task RemoveRoleUser(string roomname, string userId, string oldRole)
        {
            roomname = ValidateString(roomname);
            userId = ValidateString(userId);
            oldRole = ValidateString(oldRole);

            var sc = await _planitPokerService.RemoveRoleUser(roomname, userId, oldRole, GetConnectionId());
            if (sc)
            {
                await Clients.Group(roomname).SendAsync(UserRoleChanged, userId, 2, oldRole);
            }
        }


        public async Task AddNewStory(string roomname, string storyName, string storyDescription)
        {
            roomname = ValidateString(roomname).ToUpper();
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            //invoke AddedNewStory
            var newStory = new Story()
            {
                Name = storyName,
                Description = storyDescription,
            };

            var sc = await _planitPokerService.AddNewStory(roomname, GetConnectionId(), newStory);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(AddedNewStory, new StoryReturn(newStory));
            }
        }


        public async Task ChangeCurrentStory(string roomname, long storyId, string storyName, string storyDescription)
        {
            roomname = ValidateString(roomname).ToUpper();
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            var newStory = new Story()
            {
                Id = storyId,
                Name = storyName,
                Description = storyDescription,
            };

            var sc = await _planitPokerService.ChangeStory(roomname, GetConnectionId(), newStory);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(CurrentStoryChanged, storyId, storyName, storyDescription);//new StoryReturn(newStory)
            }
        }

        public async Task MakeCurrentStory(string roomname, long storyId)
        {
            roomname = ValidateString(roomname).ToUpper();
            //todo NewCurrentStory
            var sc = await _planitPokerService.ChangeCurrentStory(roomname, GetConnectionId(), storyId);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(NewCurrentStory, storyId);
            }
        }

        public async Task DeleteStory(string roomname, long storyId)
        {
            roomname = ValidateString(roomname).ToUpper();
            //todo DeletedStory
            var sc = await _planitPokerService.DeleteStory(roomname, GetConnectionId(), storyId);

            if (sc)
            {
                await Clients.Group(roomname).SendAsync(DeletedStory, storyId);
            }
        }


        public async Task MakeStoryComplete(string roomname, long storyId)
        {
            roomname = ValidateString(roomname).ToUpper();
            (var oldId, var story) = await _planitPokerService.MakeStoryComplete(roomname, storyId, GetConnectionId());
            if (story != null)
            {
                await Clients.Group(roomname).SendAsync(MovedStoryToComplete, oldId, new StoryReturn(story));
            }
            //
        }




        //public async Task SetCurrentStory(string roomname, string storyId)
        //{
        //}

        //public async Task RemoveStory(string roomname, string storyId)
        //{
        //}




        public async Task<bool> SaveRoom(string roomName)
        {
            roomName = ValidateString(roomName).ToUpper();
            return await _planitPokerService.SaveRoom(roomName);
        }

        public async Task DeleteRoom(string roomName)
        {
            roomName = ValidateString(roomName).ToUpper();
            var room = await _planitPokerService.DeleteRoom(roomName, GetConnectionId());
            var usersId = room.StoredRoom.Users.Select(x => x.PlaningAppUserId).ToList();
            if (usersId != null && usersId.Count > 0)
            {
                await Clients.Group(roomName).SendAsync(UserLeaved, usersId);
            }
        }





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
            var httpContext = Context.GetHttpContext();
            var cookiesHasRoomName = httpContext.Request.Cookies.TryGetValue("planing_poker_roomname", out string roomName);
            if (cookiesHasRoomName && !string.IsNullOrWhiteSpace(roomName))
            {
                roomName = ValidateString(roomName).ToUpper();
                var userConnectionId = GetConnectionId();
                (bool sc, string userId) = await _planitPokerService.LeaveFromRoom(roomName, userConnectionId);
                if (sc)
                {
                    await Clients.Group(roomName).SendAsync(UserLeaved, new List<string>() { userId });

                }
            }

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
            var userConnectionId = user.UserConnectionId;
            var userId = user.PlaningAppUserId;
            var mainAppUserId = user.MainAppUserId;

            var (sc, oldConnectionId) = await _planitPokerService.AddUserIntoRoom(room, user);
            if (!sc)
            {
                //TODO что то пошло не так
                await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });

                return false;
            }

            if (!string.IsNullOrWhiteSpace(oldConnectionId))
            {
                await Clients.Client(oldConnectionId).SendAsync(UserLeaved, new List<string>() { userId });
            }


            var roomnm = room.GetConcurentValue(_multiThreadHelper, x => x.StoredRoom.Name);
            if (!roomnm.sc)
            {
                //TODO что то пошло не так
                await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });

                return false;
            }

            //специально до добавления юзера в руму сигнала тк ему это сообщение не нужно
            var us = GetValueFromRoomAsync(room, (rm) => rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionId));
            PlanitUserReturn returnUser = null;
            if (!us.sc || us.res == null)
            {
                //TODO что то пошло не так
                await Clients.Caller.SendAsync(NotifyFromServer, new Notify() { Text = "retry plz", Status = NotyfyStatus.Error });
                return false;
            }

            returnUser = new PlanitUserReturn(us.res);

            await Clients.Group(roomnm.res).SendAsync(NewUserInRoom, returnUser);
            await Groups.AddToGroupAsync(userConnectionId, roomnm.res);

            //(var usersInRoom, bool suc) = GetValueFromRoomAsync(room, room => room.StoredRoom.Users.Select(x => x.Clone()));
            //if (!suc)
            //{
            //    //TODO отключить юзера и попросить переконнектиться
            //    return false;
            //}
            await Clients.Caller.SendAsync(EnteredInRoom, userId, userId == mainAppUserId?.ToString()); //,usersInRoom//todo мб лучше отдельным запросом?
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

        private string GenerateUniqueUserId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
