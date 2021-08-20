using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BO.Models.Auth;
using Common.Models;
using Common.Models.Error;
using Common.Models.Error.Interfaces;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Validators;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.SignalR;
using PlanitPoker.Models.Helpers;
using PlanitPoker.Models.Returns;
using PlanitPoker.Models.Services;
using Microsoft.Extensions.Logging;

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
        private readonly IPlanitApiHelper _apiHealper;
        private readonly IJWTHasher _hasher;
        private readonly IErrorService _errorService;

        private readonly IErrorContainer _errorContainer;
        private readonly ILogger _logger;

        //private static IServiceProvider _serviceProvider;

        //front endpoints








        public PlanitPokerHub(MultiThreadHelper multiThreadHelper,
            IStringValidator stringValidator, IPlanitPokerService planitPokerService,
            IPlanitApiHelper apiHealper, IJWTService jwtService, IJWTHasher hasher, IErrorService errorService
            , IErrorContainer errorContainer)
        {
            _multiThreadHelper = multiThreadHelper;
            _stringValidator = stringValidator;
            _planitPokerService = planitPokerService;

            _jwtService = jwtService;
            _hasher = hasher;
            _errorService = errorService;
            _errorContainer = errorContainer;

            _logger = null; //todo

            _apiHealper = apiHealper;
            _apiHealper.InitByHub(this);
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

        // ReSharper disable once UnusedMember.Global
        public async Task CreateRoom(string roomName, string password, string username)
        {
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                roomName = NormalizeRoomName(roomName);
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
                    (expired, userInfo) =
                        _apiHealper.GetUserInfoWithExpired(httpContext.Request, _jwtService, false);
                }
                catch
                {
                }

                if (expired && userInfo != null)
                {
                    //не прерываем процесс создания румы, но сообщаем о том что нужен рефреш
                    await Clients.Caller.SendAsync(Consts.PlanitPokerHubEndpoints.NeedRefreshTokens);
                }

                var user = new PlanitUser()
                {
                    MainAppUserId = userInfo?.UserId,
                    PlaningAppUserId = GenerateUniqueUserId(),
                    UserConnectionId = GetConnectionId(),
                    Name = username,
                    Role = GetCreatorRoles(),
                };

                var room = await _planitPokerService.CreateRoomWithUser(roomName, password, user);
                if (room == null)
                {
                    await Clients.Caller.SendAsync(Consts.PlanitPokerHubEndpoints.RoomNotCreated);

                    return;
                }

                _ = await EnterInRoom(room, user);

            }, httpContext.Response, _logger);


        }

        // ReSharper disable once UnusedMember.Global
        public async Task AliveRoom(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var room = await _planitPokerService.TryGetRoom(roomName);
                if (room == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
                }

                var newDieDate = _planitPokerService.AddTimeAliveRoom(room);
                if (newDieDate != DateTime.MinValue)
                {
                    await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.NewRoomAlive, newDieDate);
                    return;
                }


                throw new SomeCustomException(ErrorConsts.SomeError);
            }, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task EnterInRoom(string roomName, string password, string username)
        {
            roomName = NormalizeRoomName(roomName);
            username = ValidateString(username);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    password = null;
                }
                else
                {
                    password = _hasher.GetHash(password);
                }


                if (string.IsNullOrEmpty(roomName))
                {
                    _errorService.AddError(_errorContainer.TryGetError(Consts.PlanitPokerErrorConsts.RoomNameIsEmpty));
                    await _apiHealper.NotifyFromErrorService();
                    await Clients.Caller.SendAsync(Consts.PlanitPokerHubEndpoints.ConnectedToRoomError);
                }

                var room = await _planitPokerService.TryGetRoom(roomName, password);
                if (room == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
                }

                UserInfo userInfo = null;
                var expired = false;
                try
                {
                    (expired, userInfo) =
                        _apiHealper.GetUserInfoWithExpired(Context.GetHttpContext().Request, _jwtService, false);
                }
                catch
                {
                }

                if (expired && userInfo != null)
                {
                    //не прерываем процесс подключения, но сообщаем о том что нужен рефреш
                    await Clients.Caller.SendAsync(Consts.PlanitPokerHubEndpoints.NeedRefreshTokens);
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
            }, httpContext.Response, _logger);


        }

        // ReSharper disable once UnusedMember.Global
        public async Task StartVote(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                await _planitPokerService.StartVote(roomName, GetConnectionId());
                //todo Consts.PlanitPokerHubEndpoints.ConnectedToRoomError

                await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.VoteStart);

            }, httpContext.Response, _logger);
        }

        // ReSharper disable once UnusedMember.Global
        public async Task EndVote(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var result = await _planitPokerService.EndVote(roomName, GetConnectionId());
                if (result == null)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.VoteEnd, result);

            }, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task<bool> Vote(string roomName, string vote)
        {

            roomName = NormalizeRoomName(roomName);
            vote = ValidateString(vote);
            var httpContext = Context.GetHttpContext();
            return await _apiHealper.DoStandartSomething(async () =>
            {
                var room = await _planitPokerService.TryGetRoom(roomName);
                if (room == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
                }


                (bool sc1, string userId) =  _planitPokerService.ChangeVote(room, GetConnectionId(), vote);
                if (!sc1)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                //var allVoted = await _planitPokerService.AllVoted(room);
                var adminsId = await _planitPokerService.GetAdminsId(room);
                if (adminsId != null && adminsId.Count > 0)
                {
                    await Clients.Clients(adminsId).SendAsync(Consts.PlanitPokerHubEndpoints.VoteChanged, userId, vote);
                }

                //if (allVoted)
                //{
                //    var erF = new ErrorObjectReturnFactory();
                //    await Clients.Clients(adminsId).SendAsync(Consts.PlanitPokerHubEndpoints.NotifyFromServer,
                //        erF.GetObjectReturn(new ErrorObject(){Errors = new List<OneError>(){new OneError(Consts.PlanitPokerNotifyConsts.AllAreWoted, "все участники проголосовали") }}));
                //}

                await Clients.GroupExcept(roomName, adminsId)
                    .SendAsync(Consts.PlanitPokerHubEndpoints.VoteChanged, userId, "?");
                //await Clients.Caller.SendAsync(VoteSuccess, vote);
                return true;
            }, false, httpContext.Response, _logger);
        }


        // ReSharper disable once UnusedMember.Global
        public async Task KickUser(string roomName, string userId)
        {
            roomName = NormalizeRoomName(roomName);
            userId = ValidateString(userId);
            var httpContext = Context.GetHttpContext();
            string userConnectionId = GetConnectionId();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var kicked = await _planitPokerService.KickFromRoom(roomName, GetConnectionId(), userId);

                if (kicked.sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Consts.PlanitPokerHubEndpoints.UserLeaved, new List<string>() {userId});
                    await Groups.RemoveFromGroupAsync(kicked.user.UserConnectionId, roomName);

                    //if (userConnectionId == kicked.user.UserConnectionId)
                    //{
                    //    //кик самого себя надо отправить отдельно
                    //}

                    return;
                }

                throw new SomeCustomException(ErrorConsts.SomeError);
            }, httpContext.Response, _logger);




        }


        // ReSharper disable once UnusedMember.Global
        public async Task<bool> UserNameChange(string roomName, string newUserName)
        {
            roomName = NormalizeRoomName(roomName);
            newUserName = ValidateString(newUserName);
            var httpContext = Context.GetHttpContext();
            return await _apiHealper.DoStandartSomething(async () =>
            {
                string userConnectionId = GetConnectionId();
                (bool sc, string userId) =
                    await _planitPokerService.ChangeUserName(roomName, userConnectionId, newUserName);

                if (sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Consts.PlanitPokerHubEndpoints.UserNameChanged, userId, newUserName);
                    return true;
                }

                throw new SomeCustomException(ErrorConsts.SomeError);

            }, false, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task AddNewRoleToUser(string roomName, string userId, string newRole)
        {

            roomName = NormalizeRoomName(roomName);
            userId = ValidateString(userId);
            newRole = ValidateString(newRole);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.AddNewRoleToUser(roomName, userId, newRole, GetConnectionId());
                if (!sc)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                await Clients.Group(roomName)
                    .SendAsync(Consts.PlanitPokerHubEndpoints.UserRoleChanged, userId, 1, newRole);
                return;

            }, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task RemoveRoleUser(string roomname, string userId, string oldRole)
        {
            roomname = ValidateString(roomname);
            userId = ValidateString(userId);
            oldRole = ValidateString(oldRole);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.RemoveRoleUser(roomname, userId, oldRole, GetConnectionId());
                if (sc)
                {
                    await Clients.Group(roomname)
                        .SendAsync(Consts.PlanitPokerHubEndpoints.UserRoleChanged, userId, 2, oldRole);
                }
            }, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task AddNewStory(string roomName, string storyName, string storyDescription)
        {
            roomName = NormalizeRoomName(roomName);
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var newStory = new Story()
                {
                    Name = storyName,
                    Description = storyDescription,
                };

                var sc = await _planitPokerService.AddNewStory(roomName, GetConnectionId(), newStory);

                if (sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Consts.PlanitPokerHubEndpoints.AddedNewStory, new StoryReturn(newStory));
                }
            }, httpContext.Response, _logger);
            //invoke AddedNewStory

        }

        // ReSharper disable once UnusedMember.Global
        public async Task ChangeCurrentStory(string roomName, string storyId, string storyName, string storyDescription)
        {
            roomName = NormalizeRoomName(roomName);
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var storyIdIsGuid = Guid.TryParse(storyId, out var storyIdG);
                if (!storyIdIsGuid)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.StoryNotFound);
                }

                var newStory = new Story()
                {
                    TmpId = storyIdG,
                    Name = storyName,
                    Description = storyDescription,
                };

                var sc = await _planitPokerService.ChangeStory(roomName, GetConnectionId(), newStory);

                if (sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Consts.PlanitPokerHubEndpoints.CurrentStoryChanged, storyId, storyName,
                            storyDescription); //new StoryReturn(newStory)
                }
            }, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task MakeCurrentStory(string roomName, string storyId)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.ChangeCurrentStory(roomName, GetConnectionId(), storyId);

                if (sc)
                {
                    await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.NewCurrentStory, storyId);
                }
            }, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task DeleteStory(string roomName, string storyId)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.DeleteStory(roomName, GetConnectionId(), storyId);

                if (sc)
                {
                    await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.DeletedStory, storyId);
                }
            }, httpContext.Response, _logger);


        }

        // ReSharper disable once UnusedMember.Global
        public async Task MakeStoryComplete(string roomName, string storyId)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var (oldId, story) = await _planitPokerService.MakeStoryComplete(roomName, storyId, GetConnectionId());
                if (story != null)
                {
                    await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.MovedStoryToComplete, oldId,
                        new StoryReturn(story));
                }
            }, httpContext.Response, _logger);


            //
        }



        public async Task OnWindowClosedAsync(string roomName)//, string userId
        {
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                if (!string.IsNullOrWhiteSpace(roomName))
                {
                    roomName = NormalizeRoomName(roomName);
                    var userConnectionId = GetConnectionId();
                    (bool sc, string userId) = await _planitPokerService.LeaveFromRoom(roomName, userConnectionId);
                    if (sc)
                    {
                        await Groups.RemoveFromGroupAsync(userConnectionId, roomName);
                        await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.UserLeaved,
                            new List<string>() { userId });

                    }
                }

            }, httpContext.Response, _logger);


        }



        //public async Task SetCurrentStory(string roomname, string storyId)
        //{
        //}

        //public async Task RemoveStory(string roomname, string storyId)
        //{
        //}



        // ReSharper disable once UnusedMember.Global
        public async Task<bool> SaveRoom(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            return await _apiHealper.DoStandartSomething(
                async () => { return await _planitPokerService.SaveRoom(roomName, GetConnectionId()); }, false,
                httpContext.Response, _logger);
        }

        // ReSharper disable once UnusedMember.Global
        public async Task DeleteRoom(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            await _apiHealper.DoStandartSomething(async () =>
            {
                var room = await _planitPokerService.DeleteRoom(roomName, GetConnectionId());
                var usersId = room?.StoredRoom?.Users.Select(x => new {x.PlaningAppUserId, x.UserConnectionId})
                    .ToList();
                if (usersId != null && usersId.Count > 0)
                {
                    //await Clients.Group(roomName).
                    await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.UserLeaved,
                        usersId.Select(x => x.PlaningAppUserId));
                    foreach (var userConId in usersId)
                    {
                        await Groups.RemoveFromGroupAsync(userConId.UserConnectionId, roomName);
                    }
                }
            }, httpContext.Response, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task<IEnumerable<StoryReturn>> LoadNotActualStories(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            return await _apiHealper.DoStandartSomething(async () =>
            {
                var stories = await _planitPokerService.LoadNotActualStories(roomName);
                return stories.Select(x => new StoryReturn(x));
            }, new List<StoryReturn>(), httpContext.Response, _logger);

        }





        //------------------------------------ signal----------------------------------------------


        // ReSharper disable once UnusedMember.Global
        public override async Task OnConnectedAsync()
        {
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }

        // ReSharper disable once UnusedMember.Global
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //IHttpContextFeature hcf = (IHttpContextFeature)this.Context.Features[typeof(IHttpContextFeature)];
            //HttpContext hc = hcf.HttpContext;
            //string myCookieValue = hc.Request.Cookies["planing_poker_roomname"];

            //название комнаты смогу вытащить из кук???
            //var httpContext = Context.GetHttpContext();
            //var cookiesHasRoomName =
            //    httpContext.Request.Cookies.TryGetValue("planing_poker_roomname", out string roomName);
            //if (cookiesHasRoomName && !string.IsNullOrWhiteSpace(roomName))
            //{
            //    roomName = NormalizeRoomName(roomName);
            //    var userConnectionId = GetConnectionId();
            //    (bool sc, string userId) = await _planitPokerService.LeaveFromRoom(roomName, userConnectionId);
            //    if (sc)
            //    {
            //        await Groups.RemoveFromGroupAsync(userConnectionId, roomName);
            //        await Clients.Group(roomName).SendAsync(Consts.PlanitPokerHubEndpoints.UserLeaved,
            //            new List<string>() {userId});

            //    }
            //}

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

            //var username = user.Name;
            var userConnectionId = user.UserConnectionId;
            var userId = user.PlaningAppUserId;
            var mainAppUserId = user.MainAppUserId;

            var (sc, oldConnectionId) = await _planitPokerService.AddUserIntoRoom(room, user);
            if (!sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            if (!string.IsNullOrWhiteSpace(oldConnectionId))
            {
                var roomName = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Name);
                await Groups.RemoveFromGroupAsync(oldConnectionId, roomName.res);
                await Clients.Client(oldConnectionId)
                    .SendAsync(Consts.PlanitPokerHubEndpoints.UserLeaved, new List<string>() {userId});
            }


            var roomnm = room.GetConcurentValue(_multiThreadHelper, x => x.StoredRoom.Name);
            if (!roomnm.sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            //специально до добавления юзера в руму сигнала тк ему это сообщение не нужно
            var us = GetValueFromRoomAsync(room,
                (rm) => rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionId));
            if (!us.sc || us.res == null)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            var returnUser = new PlanitUserReturn(us.res);

            await Clients.Group(roomnm.res).SendAsync(Consts.PlanitPokerHubEndpoints.NewUserInRoom, returnUser);
            await Groups.AddToGroupAsync(userConnectionId, roomnm.res);

            await Clients.Caller.SendAsync(Consts.PlanitPokerHubEndpoints.EnteredInRoom, userId,
                userId == mainAppUserId?.ToString()); //,usersInRoom//todo мб лучше отдельным запросом?
            return true;
        }


        private List<string> GetDefaultRoles()
        {
            return new List<string>() {Consts.Roles.User};
        }

        private List<string> GetCreatorRoles()
        {
            return new List<string>(GetDefaultRoles()) {Consts.Roles.Creator, Consts.Roles.Admin};
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

        private string NormalizeRoomName(string roomName)
        {
            return ValidateString(roomName).ToUpper();
        }
    }
}
