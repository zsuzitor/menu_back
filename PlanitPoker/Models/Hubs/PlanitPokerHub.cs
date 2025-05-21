using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BO.Models.Auth;
using Common.Models;
using Common.Models.Error;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Validators;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.SignalR;
using PlanitPoker.Models.Helpers;
using PlanitPoker.Models.Returns;
using PlanitPoker.Models.Services;
using Microsoft.Extensions.Logging;
using PlanitPoker.Models.Entity;
using System.Text.RegularExpressions;
using Menu.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using BL.Models.Services.Interfaces;

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
        private readonly IHasher _hasher;
        private readonly IErrorService _errorService;

        private readonly IConfigurationService _configurationService;
        private readonly ILogger _logger;
        private readonly ILogger _hublogger;
        private readonly IUserService _userService;


        //private static IServiceProvider _serviceProvider;

        //front endpoints



        public PlanitPokerHub(MultiThreadHelper multiThreadHelper,
            IStringValidator stringValidator, IPlanitPokerService planitPokerService,
            IUserService userService,
            IPlanitApiHelper apiHealper, IJWTService jwtService, IHasher hasher, IErrorService errorService
            , IConfigurationService configurationService, ILogger<PlanitPokerHub> logger, ILoggerFactory loggerFactory)
        {
            _multiThreadHelper = multiThreadHelper;
            _stringValidator = stringValidator;
            _planitPokerService = planitPokerService;
            _userService = userService;

            _jwtService = jwtService;
            _hasher = hasher;
            _errorService = errorService;
            _configurationService = configurationService;

            _logger = logger;
            _hublogger = loggerFactory.CreateLogger("PlanitPoker");

            _apiHealper = apiHealper;
            _apiHealper.InitByHub(this);
        }



        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        // ReSharper disable once UnusedMember.Global
        public async Task CreateRoom(string roomName, string password, string username)
        {
            roomName = NormalizeRoomName(roomName);
            var httpContext = Context.GetHttpContext();
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(CreateRoom), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                ValidateRoomName(roomName);
                username = ValidateString(username);
                ValidateUserName(username);
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
                User userMainAppInfo = null;
                try
                {
                    (expired, userInfo) =
                        _apiHealper.GetUserInfoWithExpired(httpContext.Request, _jwtService, false);
                    userMainAppInfo = await _userService.GetShortInfoAsync(userInfo.UserId);

                }
                catch
                {
                }

                if (expired && userInfo != null)
                {
                    //не прерываем процесс создания румы, но сообщаем о том что нужен рефреш
                    await Clients.Caller.SendAsync(Constants.PlanitPokerHubEndpoints.NeedRefreshTokens);
                }

                var user = new PlanitUser()
                {
                    MainAppUserId = userInfo?.UserId,
                    PlaningAppUserId = GenerateUniqueUserId(),
                    UserConnectionId = connectionId,
                    Name = username,
                    Role = GetCreatorRoles(),
                    ImageLink = userMainAppInfo?.ImagePath,
                };

                var room = await _planitPokerService.CreateRoomWithUserAsync(roomName, password, user);
                if (room == null)
                {
                    await Clients.Caller.SendAsync(Constants.PlanitPokerHubEndpoints.RoomNotCreated);

                    return;
                }

                _ = await EnterInRoom(room, user);

            }, _logger);


        }

        // ReSharper disable once UnusedMember.Global
        public async Task AliveRoom(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(AliveRoom), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var room = await _planitPokerService.TryGetRoomAsync(roomName);
                if (room == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
                }

                var newDieDate = await _planitPokerService.AddTimeAliveRoom(room);
                if (newDieDate != DateTime.MinValue)
                {
                    await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.NewRoomAlive, newDieDate);
                    return;
                }


                throw new SomeCustomException(ErrorConsts.SomeError);
            }, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task SetRoomCards(string roomName, string cards)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            cards = ValidateString(cards);
            Log(LogLevel.Debug, nameof(SetRoomCards), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                if (string.IsNullOrWhiteSpace(cards))
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomBadVoteMarks);
                }

                var rg = new Regex(@"^[a-zA-Z0-9;\.]*$");
                if (!rg.IsMatch(cards))
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomBadVoteMarks);
                }

                var room = await _planitPokerService.TryGetRoomAsync(roomName);

                var cardsList = cards.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();

                var rs = await _planitPokerService.SetRoomCards(room, connectionId, cardsList);
                if (!rs)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.RoomCardsChanged, cardsList);
                var result = await _planitPokerService.RecalcEndVoteInfo(room);
                if (result == null)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                //todo зачем это?
                await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.VoteEnd, result);

            }, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task EnterInRoom(string roomName, string password, string username)
        {
            roomName = NormalizeRoomName(roomName);
            username = ValidateString(username);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(EnterInRoom), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                ValidateUserName(username);

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
                    _errorService.AddError(Constants.PlanitPokerErrorConsts.RoomNameIsEmpty, 
                        (await _configurationService.GetAsync(Constants.PlanitPokerErrorConsts.RoomNameIsEmpty)).Value);
                    await _apiHealper.NotifyFromErrorService();
                    await Clients.Caller.SendAsync(Constants.PlanitPokerHubEndpoints.ConnectedToRoomError);
                }

                var room = await _planitPokerService.TryGetRoomAsync(roomName, password);
                if (room == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
                }

                UserInfo userInfo = null;
                var expired = false;
                User userMainAppInfo = null;
                try
                {
                    (expired, userInfo) =
                        _apiHealper.GetUserInfoWithExpired(Context.GetHttpContext().Request, _jwtService, false);
                    userMainAppInfo = await _userService.GetShortInfoAsync(userInfo.UserId);
                }
                catch
                {
                }

                if (expired && userInfo != null)
                {
                    //не прерываем процесс подключения, но сообщаем о том что нужен рефреш
                    await Clients.Caller.SendAsync(Constants.PlanitPokerHubEndpoints.NeedRefreshTokens);
                }

                var user = new PlanitUser()
                {
                    MainAppUserId = userInfo?.UserId,
                    PlaningAppUserId = GenerateUniqueUserId(),
                    UserConnectionId = connectionId,
                    Name = username,
                    Role = GetDefaultRoles(),
                    ImageLink = userMainAppInfo?.ImagePath,
                };

                _ = await EnterInRoom(room, user);
            }, _logger);


        }

        // ReSharper disable once UnusedMember.Global
        public async Task StartVote(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(StartVote), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                await _planitPokerService.StartVoteAsync(roomName, connectionId);
                //todo Consts.PlanitPokerHubEndpoints.ConnectedToRoomError

                await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.VoteStart);

            }, _logger);
        }

        // ReSharper disable once UnusedMember.Global
        public async Task EndVote(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(EndVote), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var result = await _planitPokerService.EndVoteAsync(roomName, connectionId);
                if (result == null)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.VoteEnd, result);

            }, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task<bool> Vote(string roomName, string vote)
        {

            roomName = NormalizeRoomName(roomName);
            vote = ValidateString(vote);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(Vote), "InvokeLog", roomName, connectionId, string.Empty);
            return await _apiHealper.DoStandartSomething(async () =>
            {
                var room = await _planitPokerService.TryGetRoomAsync(roomName);
                if (room == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
                }


                (bool sc1, string userId) = await _planitPokerService.ChangeVote(room, connectionId, vote);
                if (!sc1)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                //var allVoted = await _planitPokerService.AllVoted(room);
                var adminsId = await _planitPokerService.GetAdminsIdAsync(room);
                if (adminsId != null && adminsId.Count > 0)
                {
                    await Clients.Clients(adminsId).SendAsync(Constants.PlanitPokerHubEndpoints.VoteChanged, userId, vote);
                }

                //if (allVoted)
                //{
                //    var erF = new ErrorObjectReturnFactory();
                //    await Clients.Clients(adminsId).SendAsync(Consts.PlanitPokerHubEndpoints.NotifyFromServer,
                //        erF.GetObjectReturn(new ErrorObject(){Errors = new List<OneError>(){new OneError(Consts.PlanitPokerNotifyConsts.AllAreWoted, "все участники проголосовали") }}));
                //}

                await Clients.GroupExcept(roomName, adminsId)
                    .SendAsync(Constants.PlanitPokerHubEndpoints.VoteChanged, userId, "?");
                //await Clients.Caller.SendAsync(VoteSuccess, vote);
                return true;
            }, false, _logger);
        }


        // ReSharper disable once UnusedMember.Global
        public async Task KickUser(string roomName, string userId)
        {
            roomName = NormalizeRoomName(roomName);
            userId = ValidateString(userId);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(KickUser), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var kicked = await _planitPokerService.KickFromRoomAsync(roomName, connectionId, userId);

                if (kicked.sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Constants.PlanitPokerHubEndpoints.UserLeaved, new List<string>() { userId });
                    await Groups.RemoveFromGroupAsync(kicked.user.UserConnectionId, roomName);

                    //if (userConnectionId == kicked.user.UserConnectionId)
                    //{
                    //    //кик самого себя надо отправить отдельно
                    //}

                    return;
                }

                throw new SomeCustomException(ErrorConsts.SomeError);
            }, _logger);




        }


        // ReSharper disable once UnusedMember.Global
        public async Task<bool> UserNameChange(string roomName, string newUserName)
        {
            roomName = NormalizeRoomName(roomName);
            newUserName = ValidateString(newUserName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(UserNameChange), "InvokeLog", roomName, connectionId, string.Empty);
            return await _apiHealper.DoStandartSomething(async () =>
            {
                ValidateUserName(newUserName);

                (bool sc, string userId) =
                    await _planitPokerService.ChangeUserNameAsync(roomName, connectionId, newUserName);

                if (sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Constants.PlanitPokerHubEndpoints.UserNameChanged, userId, newUserName);
                    return true;
                }

                throw new SomeCustomException(ErrorConsts.SomeError);

            }, false, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task AddNewRoleToUser(string roomName, string userId, string newRole)
        {
            roomName = NormalizeRoomName(roomName);
            userId = ValidateString(userId);
            newRole = ValidateString(newRole);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(AddNewRoleToUser), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.AddNewRoleToUserAsync(roomName, userId, newRole, connectionId);
                if (!sc)
                {
                    throw new SomeCustomException(ErrorConsts.SomeError);
                }

                await Clients.Group(roomName)
                    .SendAsync(Constants.PlanitPokerHubEndpoints.UserRoleChanged, userId, 1, newRole);
                return;

            }, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task RemoveRoleUser(string roomName, string userId, string oldRole)
        {
            //roomname = ValidateString(roomname);
            roomName = NormalizeRoomName(roomName);
            userId = ValidateString(userId);
            oldRole = ValidateString(oldRole);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(RemoveRoleUser), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.RemoveRoleUserAsync(roomName, userId, oldRole, connectionId);
                if (sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Constants.PlanitPokerHubEndpoints.UserRoleChanged, userId, 2, oldRole);
                }
            }, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task AddNewStory(string roomName, string storyName, string storyDescription)
        {
            roomName = NormalizeRoomName(roomName);
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(AddNewStory), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var newStory = new Story()
                {
                    Name = storyName,
                    Description = storyDescription,
                    CurrentSession = true,
                };

                var sc = await _planitPokerService.AddNewStoryAsync(roomName, connectionId, newStory);

                if (sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Constants.PlanitPokerHubEndpoints.AddedNewStory, new StoryReturn(newStory));
                }
            }, _logger);
            //invoke AddedNewStory

        }

        // ReSharper disable once UnusedMember.Global
        public async Task ChangeCurrentStory(string roomName, string storyId, string storyName, string storyDescription)
        {
            roomName = NormalizeRoomName(roomName);
            storyName = ValidateString(storyName);
            storyDescription = ValidateString(storyDescription);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(ChangeCurrentStory), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var newStory = new Story()
                {
                    Name = storyName,
                    Description = storyDescription,
                    CurrentSession = true,
                };

                if (Guid.TryParse(storyId, out var storyIdG))
                {
                    newStory.TmpId = storyIdG;
                }
                else if (long.TryParse(storyId, out var idLong))
                {
                    newStory.IdDb = idLong;
                }

                if (string.IsNullOrEmpty(newStory.Id))
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.StoryNotFound);
                }

                var sc = await _planitPokerService.ChangeStoryAsync(roomName, connectionId, newStory);

                if (sc)
                {
                    await Clients.Group(roomName)
                        .SendAsync(Constants.PlanitPokerHubEndpoints.CurrentStoryChanged, storyId, storyName,
                            storyDescription); //new StoryReturn(newStory)
                }
            }, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task MakeCurrentStory(string roomName, string storyId)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(MakeCurrentStory), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.ChangeCurrentStoryAsync(roomName, connectionId, storyId);

                if (sc)
                {
                    await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.NewCurrentStory, storyId);
                }
            }, _logger);

        }

        // ReSharper disable once UnusedMember.Global
        public async Task DeleteStory(string roomName, string storyId)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(DeleteStory), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var sc = await _planitPokerService.DeleteStoryAsync(roomName, connectionId, storyId);

                if (sc)
                {
                    await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.DeletedStory, storyId);
                }
            }, _logger);


        }

        // ReSharper disable once UnusedMember.Global
        public async Task MakeStoryComplete(string roomName, string storyId)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(MakeStoryComplete), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomething(async () =>
            {
                var (oldId, story) = await _planitPokerService.MakeStoryCompleteAsync(roomName, storyId, GetConnectionId());
                if (story != null)
                {
                    await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.MovedStoryToComplete, oldId,
                        new StoryReturn(story));
                }
            }, _logger);


            //
        }



        public async Task OnWindowClosedAsync(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(OnWindowClosedAsync), "InvokeLog", roomName, connectionId, string.Empty);
            await _apiHealper.DoStandartSomethingWithoutResponse(async () =>
            {
                if (!string.IsNullOrWhiteSpace(roomName))
                {
                    (bool sc, string userId) = await _planitPokerService.LeaveFromRoomAsync(roomName, connectionId);
                    if (sc)
                    {
                        await Groups.RemoveFromGroupAsync(connectionId, roomName);
                        await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.UserLeaved,
                            new List<string>() { userId });

                    }
                }

                return true;

            }, true, _logger);


        }




        // ReSharper disable once UnusedMember.Global
        public async Task<bool> SaveRoom(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(SaveRoom), "InvokeLog", roomName, connectionId, string.Empty);
            return await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var res = await _planitPokerService.SaveRoomAsync(roomName, connectionId);
                    if (res.Success)
                    {
                        await Clients.Group(roomName)
                         .SendAsync(Constants.PlanitPokerHubEndpoints.RoomWasSaved, res);

                    }

                    return res.Success;
                }, false, _logger);
        }

        // ReSharper disable once UnusedMember.Global
        public async Task DeleteRoom(string roomName)
        {
            roomName = NormalizeRoomName(roomName);
            string connectionId = GetConnectionId();
            Log(LogLevel.Debug, nameof(DeleteRoom), "InvokeLog", roomName, connectionId, string.Empty);

            await _apiHealper.DoStandartSomething(async () =>
            {
                var room = await _planitPokerService.DeleteRoomAsync(roomName, connectionId);
                var usersId = room?.StoredRoom?.Users.Select(x => new { x.PlaningAppUserId, x.UserConnectionId })
                    .ToList();
                if (usersId != null && usersId.Count > 0)
                {
                    //await Clients.Group(roomName).
                    await Clients.Group(roomName).SendAsync(Constants.PlanitPokerHubEndpoints.UserLeaved,
                        usersId.Select(x => x.PlaningAppUserId));
                    foreach (var userConId in usersId)
                    {
                        await Groups.RemoveFromGroupAsync(userConId.UserConnectionId, roomName);
                    }
                }
            }, _logger);

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

            var (sc, oldConnectionId) = await _planitPokerService.AddUserIntoRoomAsync(room, user);
            if (!sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            if (!string.IsNullOrWhiteSpace(oldConnectionId))
            {
                var roomName = await room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Name);
                await Groups.RemoveFromGroupAsync(oldConnectionId, roomName.res);
                await Clients.Client(oldConnectionId)
                    .SendAsync(Constants.PlanitPokerHubEndpoints.UserLeaved, new List<string>() { userId });
            }


            var roomnm = await room.GetConcurentValue(_multiThreadHelper, x => x.StoredRoom.Name);
            if (!roomnm.sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            //специально до добавления юзера в руму сигнала тк ему это сообщение не нужно
            var us = await GetValueFromRoomAsync(room,
                (rm) => rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionId));
            if (!us.sc || us.res == null)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            var returnUser = new PlanitUserReturn(us.res);

            await Clients.Group(roomnm.res).SendAsync(Constants.PlanitPokerHubEndpoints.NewUserInRoom, returnUser);
            await Groups.AddToGroupAsync(userConnectionId, roomnm.res);

            await Clients.Caller.SendAsync(Constants.PlanitPokerHubEndpoints.EnteredInRoom, userId,
                userId == mainAppUserId?.ToString()); //,usersInRoom//todo мб лучше отдельным запросом?
            return true;
        }


        private List<string> GetDefaultRoles()
        {
            return new List<string>() { Constants.Roles.User };
        }

        private List<string> GetCreatorRoles()
        {
            return new List<string>(GetDefaultRoles()) { Constants.Roles.Creator, Constants.Roles.Admin };
        }



        private async Task<(T res, bool sc)> GetValueFromRoomAsync<T>(Room room, Func<Room, T> get)
        {
            return await room.GetConcurentValue(_multiThreadHelper, get);
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
            return ValidateString(roomName)?.ToUpper();
        }

        private void ValidateRoomName(string roomName)
        {
            var rg = new Regex("^[a-zA-Z0-9_]{1,30}$");
            if (!rg.Match(roomName).Success)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.BadRoomNameWithRoomCreating);
            }
        }

        private void ValidateUserName(string userName)
        {
            var rg = new Regex("^[а-яА-Яa-zA-Z0-9_@. ]{1,50}$");
            if (!rg.Match(userName).Success)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.UsernameBad);
            }
        }


        private void Log(LogLevel lvl, string action, string message, string roomName, string connectionId, string userId)
        {
            var config = new Dictionary<string, object>();
            config.Add("action", $"PlanitPoker-{action}");
            config.Add("connection_id", connectionId);
            config.Add("group_name", roomName);
            config.Add("user_id", userId);
            Log(lvl, message, config);
        }

        private void Log(LogLevel lvl, string message, Dictionary<string, object> config)
        {
            using (_hublogger.BeginScope(config))
            {
                _hublogger.Log(lvl, message);
            }
        }
    }
}
