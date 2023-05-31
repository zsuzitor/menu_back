using BO.Models.PlaningPoker.DAL;
using Common.Models;
using Common.Models.Error.Interfaces;
using Common.Models.Error.services.Interfaces;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Returns;
using PlanitPoker.Models.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Exceptions;
using Common.Models.Error;
using System.Globalization;
using DAL.Models.DAL;
using jwtLib.JWTAuth.Interfaces;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Menu.Models.Services.Interfaces;
using BL.Models.Services.Interfaces;

namespace PlanitPoker.Models.Services
{
    public sealed class PlanitPokerService : IPlanitPokerService
    {
        private static readonly ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();

        private readonly MenuDbContext _db;


        private readonly MultiThreadHelper _multiThreadHelper;

        private readonly IRoomRepository _roomRepository;
        private readonly IPlaningUserRepository _planingUserRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IErrorService _errorService;
        private readonly IErrorContainer _errorContainer;
        private readonly IHasher _hasher;
        private readonly IImageService _imageService;




        private readonly DBHelper _dbHelper;


        private static readonly List<string> DefaultCards = new List<string>()
            {
                "0.5", "1", "2", "3", "5", "7", "10", "13", "15", "18", "20", "25", "30", "35", "40", "50", "tea"
            };


        public PlanitPokerService(
                MultiThreadHelper multiThreadHelper,
                IRoomRepository roomRepository, IStoryRepository storyRepository
                , IErrorService errorService, IErrorContainer errorContainer,
                 IHasher hasher,
                DBHelper dbHelper, MenuDbContext db,
                IPlaningUserRepository planingUserRepository,
                IImageService imageService
            )
        {
            _multiThreadHelper = multiThreadHelper;
            _roomRepository = roomRepository;
            _storyRepository = storyRepository;
            _planingUserRepository = planingUserRepository;
            _imageService = imageService;

            _errorService = errorService;
            _errorContainer = errorContainer;
            _hasher = hasher;
            _dbHelper = dbHelper;
            _db = db;
        }

        public async Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userConnectionId)
        {
            var users = await GetAllUsers(room);
            if (users == null || users.Count == 0)
            {
                return new List<PlanitUser>();
            }

            var roomStatusR = await room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Status);
            if (!roomStatusR.sc)
            {
                return new List<PlanitUser>();
            }

            return ClearHideData(roomStatusR.res, userConnectionId, users);
        }

        public async Task<List<PlanitUser>> GetAllUsersWithRightAsync(string roomName, string userConnectionId)
        {
            var room = await TryGetRoomAsync(roomName);
            return await GetAllUsersWithRight(room, userConnectionId);
        }

        public async Task<RoomInfoReturn> GetRoomInfoWithRightAsync(string roomName, string userConnectionId)
        {
            var room = await TryGetRoomAsync(roomName);
            return await GetRoomInfoWithRight(room, userConnectionId);
        }

        public async Task<RoomInfoReturn> GetRoomInfoWithRight(Room room, string userConnectionId)
        {
            if (string.IsNullOrWhiteSpace(userConnectionId))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            //todo это можно сделать без локов тк ниже рума полностью копируется, можно создать новый метод
            var roomInfo = await GetEndVoteInfo(room);
            var scRoom = await room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Clone());
            if (!scRoom.sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            //все склонированное, работаем обычно
            var resRoom = scRoom.res;
            resRoom.Password = null;
            resRoom.Users = ClearHideData(resRoom.Status, userConnectionId, resRoom.Users);
            var res = new RoomInfoReturn()
            {
                Room = new StoredRoomReturn(resRoom),
                EndVoteInfo = roomInfo,
            };
            return res;
        }


        public async Task<EndVoteInfo> GetEndVoteInfoAsync(string roomName)
        {
            var room = await TryGetRoomAsync(roomName);
            return await GetEndVoteInfo(room);

        }

        public async Task<EndVoteInfo> GetEndVoteInfo(Room room)
        {
            var res = await room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                return new EndVoteInfo(rm.StoredRoom.EndVoteInfo);
            });
            return res.res;
        }

        public async Task<EndVoteInfo> RecalcEndVoteInfo(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            (var res, bool sc) = await room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                if (rm.StoredRoom.Status != RoomSatus.CloseVote)
                {
                    return null;
                }

                return rm.StoredRoom.Users.Where(x => !string.IsNullOrWhiteSpace(x.UserConnectionId))
                    .Select(x =>
                        new { userId = x.PlaningAppUserId, vote = x.Vote, hasVote = !string.IsNullOrWhiteSpace(x.Vote) })
                    .ToList();
            });

            if (!sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            if (res == null)
            {
                //await room.SetConcurentValue(_multiThreadHelper,
                //    rm =>
                //    {
                //        rm.StoredRoom.EndVoteInfo = new EndVoteInfo();
                //    });
                return null;
            }

            NumberStyles styles = NumberStyles.Number;
            var cultureInfo = new CultureInfo("en-US");
            var arrForMath = res.Where(x => x.hasVote && decimal.TryParse(x.vote?.Replace('.', ','), styles, cultureInfo, out _))
                .Select(x => decimal.Parse(x.vote, styles, cultureInfo))
                .ToList();
            var result = new EndVoteInfo();
            if (arrForMath.Any())
            {
                result.MinVote = arrForMath.Min(x => x);
                result.MaxVote = arrForMath.Max(x => x);
                result.Average = arrForMath.Average(x => x);
            }

            result.UsersInfo = res.Select(x => new EndVoteUserInfo() { Id = x.userId, Vote = x.vote }).ToList();
            await room.SetConcurentValue(_multiThreadHelper,
                    rm =>
                    {
                        rm.StoredRoom.EndVoteInfo = new EndVoteInfo(result);
                    });
            return result;
        }




        public async Task<Room> DeleteRoomAsync(string roomName, string userConnectionIdRequest)
        {
            var isAdmin = await UserIsAdminAsync(roomName, userConnectionIdRequest);
            if (!isAdmin)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.DontHaveAccess);

            }

            if (!Rooms.Remove(roomName, out var room))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            await _roomRepository.DeleteByNameAsync(roomName);
            return room;
        }

        public async Task<RoomWasSavedUpdate>
            SaveRoomAsync(string roomName, string userConnectionIdRequest)
        {
            //только из памяти берем
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            await room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                var currentUserFromRoom = room.StoredRoom.Users.FirstOrDefault(x =>
                    x.UserConnectionId == userConnectionIdRequest && x.IsAdmin && x.MainAppUserId != null);
                if (currentUserFromRoom == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.DontHaveAccess);
                }

                return true;
            });

            return await SaveRoomWithoutRights(room);
        }


        public async Task<DateTime> AddTimeAliveRoomAsync(string roomName)
        {
            var room = await TryGetRoomAsync(roomName);
            return await AddTimeAliveRoom(room);
        }

        public async Task<DateTime> AddTimeAliveRoom(Room room)
        {
            var res = DateTime.MinValue;
            await room.SetConcurentValue(_multiThreadHelper,
                rm =>
                {
                    res = DateTime.Now.AddHours(Constants.DefaultHourRoomAlive);
                    rm.StoredRoom.DieDate = res;
                });

            return res;
        }



        public async Task<(bool sc, string oldConnectionId)> AddUserIntoRoomAsync(string roomName, PlanitUser user)
        {
            var room = await TryGetRoomAsync(roomName);
            return await AddUserIntoRoomAsync(room, user);
        }

        public async Task<(bool sc, string oldConnectionId)> AddUserIntoRoomAsync(Room room, PlanitUser user)
        {

            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(user?.UserConnectionId) ||
                string.IsNullOrWhiteSpace(user.Name))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            string oldConnectionId = null;
            var success = await room.SetConcurentValueAsync(_multiThreadHelper, rm =>
            {
                var nowPlus = DateTime.Now.AddMinutes(5);
                if (rm.StoredRoom.DieDate < nowPlus)
                {
                    //это нужно на случай если таймер уже закончился, но рума не очищена,
                    //в таком случае как только ты в нее заходишь тебя выкидывает по таймеру
                    //todo в иделе надо всем пользакам обновить время, но это не критично
                    rm.StoredRoom.DieDate = nowPlus;
                }

                var us = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == user.UserConnectionId
                                                                 || (user.MainAppUserId.HasValue &&
                                                                     x.MainAppUserId == user.MainAppUserId));
                if (us == null)
                {
                    rm.StoredRoom.Users.Add(user);
                }
                else
                {
                    us.Name = user.Name;
                    oldConnectionId = us.UserConnectionId;
                    us.UserConnectionId = user.UserConnectionId;
                    us.MainAppUserId ??= user.MainAppUserId;
                    us.ImageLink = user.ImageLink;
                }

                return Task.CompletedTask;
            });

            return (success, oldConnectionId);
            //return true;
        }

        public async Task<bool> ChangeStatusIfCanAsync(string roomName, string userConnectionIdRequest, RoomSatus newStatus)
        {
            var room = await TryGetRoomAsync(roomName);
            return await ChangeStatusIfCanAsync(room, userConnectionIdRequest, newStatus);
        }

        public async Task<bool> ChangeStatusIfCanAsync(Room room, string userConnectionIdRequest, RoomSatus newStatus)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(userConnectionIdRequest))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            return await UpdateIfCan(room, userConnectionIdRequest, true, rm =>
            {
                rm.Status = newStatus;
                return Task.FromResult(true);
            });
        }

        public async Task<(bool sc, string userId)> ChangeVote(Room room, string connectionUserId, string vote)
        {
            if (room == null)
            {

                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(connectionUserId))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            bool result = false;
            string userId = null;
            var suc = await room.SetConcurentValue(_multiThreadHelper, rm =>
            {
                if (rm.StoredRoom.Status != RoomSatus.AllCanVote)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.CantVote);
                }

                if (rm.StoredRoom.Cards.FirstOrDefault(x => x?.Equals(vote) ?? false) == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomBadVoteMark);
                }

                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == connectionUserId);
                if (user == null || !user.CanVote)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.CantVote);
                }

                userId = user.PlaningAppUserId;
                user.Vote = vote;
                result = true;
            });

            ThrowBySuccess(suc);

            return (result, userId);
        }



        public async Task<bool> ClearVotes(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            return await room.SetConcurentValue(_multiThreadHelper,
                rm => {
                    rm.StoredRoom.Users.ForEach(x => { x.Vote = null; });
                    rm.StoredRoom.EndVoteInfo = new EndVoteInfo();
                });
        }

        public async Task<Room> CreateRoomWithUserAsync(string roomName, string password, PlanitUser user)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNameIsEmpty);
            }

            if (user == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }

            if (Rooms.ContainsKey(roomName))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomAlreadyExist);
            }

            var roomFromDb = await _roomRepository.ExistAsync(roomName);
            if (roomFromDb) // != null
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomAlreadyExist);
            }

            var roomData = new StoredRoom(roomName, password);
            var room = new Room(roomData);
            roomData.Cards = DefaultCards.Select(x => x).ToList();

            var added = Rooms.TryAdd(roomName, room);
            if (added)
            {
                return room;
            }

            throw new SomeCustomException(Constants.PlanitPokerErrorConsts.SomeErrorWithRoomCreating);
        }

        public async Task<List<string>> GetAdminsIdAsync(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            var res = await room.GetConcurentValue(_multiThreadHelper,
                rm => rm.StoredRoom.Users.Where(x => x.IsAdmin).Select(x => x.UserConnectionId).ToList());
            if (!res.sc)
            {
                return new List<string>();
            }

            return res.res;
        }

        public async Task<List<string>> GetAdminsIdAsync(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return new List<string>();
            }

            var room = await TryGetRoomAsync(roomName);
            return await GetAdminsIdAsync(room);
        }

        /// <summary>
        /// возвращает копию пользователей
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<List<PlanitUser>> GetAllUsers(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }


            (var usersInRoom, bool suc) = await room.GetConcurentValue(_multiThreadHelper,
                rm => rm.StoredRoom.Users.Select(x => x.Clone()).ToList());
            if (!suc)
            {
                //TODO отключить юзера и попросить переконнектиться?
                return new List<PlanitUser>();
            }

            return usersInRoom;
        }

        /// <summary>
        /// возвращает копию пользователей
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public async Task<List<PlanitUser>> GetAllUsersAsync(string roomName)
        {
            var room = await TryGetRoomAsync(roomName);
            return await GetAllUsers(room);
        }



        

        /// <summary>
        /// тянет еще и из бд
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Room> TryGetRoomAsync(string roomName, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }

            var room = await TryGetRoomAsync(roomName, false);
            if (room == null)
            {
                return null;
            }

            var storedPs = await room.GetConcurentValue(_multiThreadHelper, (rm) => rm.StoredRoom.Password);
            if (!storedPs.sc)
            {
                return null;
            }

            if (storedPs.res == password)// todo eql?
            {
                return room;
            }

            return null;
        }

        public async Task<(bool sc, string userId)> ChangeUserNameAsync(string roomName, string connectionUserId,
            string newUserName)
        {
            if (string.IsNullOrWhiteSpace(connectionUserId) || string.IsNullOrWhiteSpace(newUserName))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            bool result = false;
            string userId = null;
            var suc = await room.SetConcurentValue(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == connectionUserId);
                if (user == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
                }

                user.Name = newUserName;
                userId = user.PlaningAppUserId;
                result = true;
            });


            ThrowBySuccess(suc);


            return (result, userId);
        }

        public async Task<Room> TryGetRoomAsync(string roomName, bool cacheOnly = true)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return null;
            }

            var exist = Rooms.TryGetValue(roomName, out var room);
            if (!exist)
            {
                if (cacheOnly)
                {
                    return null;
                }

                var roomFromDb = await _roomRepository.GetByNameAsync(roomName);


                var dbRoom = await GetRoomFromDbObject(roomFromDb);
                if (dbRoom == null)
                {
                    return null;
                }

                var sessionStoriesIds = dbRoom.StoredRoom.Stories
                    .Where(x => x.IdDb != null).Select(x => x.IdDb.Value).ToList();

                //todo конечно не прям хорошо, тк грузим лишние данные а нам даже пароль может не подойти
                dbRoom.StoredRoom.TotalNotActualStoriesCount
                    = await _storyRepository.GetCountNotActualForRoomAsync
                        (dbRoom.StoredRoom.Id.Value, sessionStoriesIds);
                var addedToCache = Rooms.TryAdd(roomName, dbRoom);
                if (!addedToCache)
                {
                    return null;
                }

                room = dbRoom;
            }

            return room;
        }

        public async Task<bool> UserIsAdminAsync(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoomAsync(roomName);
            return await UserIsAdmin(room, userConnectionIdRequest);
        }

        public async Task<bool> UserIsAdmin(Room room, string userConnectionIdRequest)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(userConnectionIdRequest))
            {
                return false;
            }

            var res = await room.GetConcurentValue(_multiThreadHelper,
                rm => rm.StoredRoom.Users.Any(x => x.UserConnectionId == userConnectionIdRequest && x.IsAdmin));
            return res.sc && res.res;
        }

        //вернет true только если роль была именно добавлена
        public async Task<bool> AddNewRoleToUserAsync(string roomName, string userId, string newRole,
            string userConnectionIdRequest)
        {
            if (!Constants.Roles.IsValideRole(newRole))
            {
                return false;
            }

            return await UpdateUserIfCan(roomName, userId, userConnectionIdRequest, (user) =>
            {
                if (!user.Role.Contains(newRole))
                {
                    user.Role.Add(newRole);
                    if (!user.CanVote)
                    {
                        user.Vote = null;
                    }

                    return true;
                }

                return false;
            });



        }

        public async Task<bool> RemoveRoleUserAsync(string roomName, string userId, string oldRole,
            string userConnectionIdRequest)
        {
            if (!Constants.Roles.IsValideRole(oldRole))
            {
                return false;
            }

            return await UpdateUserIfCan(roomName, userId, userConnectionIdRequest, (user) =>
            {
                if (user.Role.Contains(oldRole))
                {
                    user.Role.Remove(oldRole);
                    return true;
                }

                return false;
            });
        }



        public async Task StartVoteAsync(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            var success =
                await ChangeStatusIfCanAsync(room, userConnectionIdRequest, Enums.RoomSatus.AllCanVote);
            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            success = await ClearVotes(room);
            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }
        }

        public async Task<EndVoteInfo> EndVoteAsync(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            var success = await ChangeStatusIfCanAsync(room, userConnectionIdRequest, RoomSatus.CloseVote);
            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            var result = await RecalcEndVoteInfo(room);
            return result;

        }


        public async Task<bool> AddNewStoryAsync(string roomName, string userConnectionIdRequest, Story newStory)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, true, (room) =>
            {
                newStory.TmpId = Guid.NewGuid();
                room.Stories.Add(newStory);
                return Task.FromResult(true);
            });

        }

        public async Task<bool> ChangeStoryAsync(string roomName, string userConnectionIdRequest, Story newData)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, true, async (room) =>
            {
                var story = room.Stories.FirstOrDefault(x => x.Id == newData.Id);
                if (story == null)
                {
                    return false;
                }

                story.Name = newData.Name;
                story.Description = newData.Description;

                if (story.IdDb != null)
                {
                    await _storyRepository.UpdateAsync(story.IdDb.Value, newData.Name, newData.Description);
                }

                return true;
            });
        }

        public async Task<bool> ChangeCurrentStoryAsync(string roomName, string userConnectionIdRequest, string storyId)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, true, (room) =>
            {
                var story = room.Stories.FirstOrDefault(x => x.Id == storyId);
                if (story == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.StoryNotFound);
                }

                room.CurrentStoryId = storyId;
                return Task.FromResult(true);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="userConnectionIdRequest"></param>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteStoryAsync(string roomName, string userConnectionIdRequest, string storyId)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, true, async (room) =>
            {
                if (room.CurrentStoryId == storyId)
                {
                    room.CurrentStoryId = string.Empty;
                }

                var storyForDel = room.Stories.FirstOrDefault(x => x.Id == storyId);
                if (storyForDel == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.StoryNotFound);
                }

                if (storyForDel.Completed)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.StoryBadStatus);
                }

                if (storyForDel.IdDb != null)
                {
                    await _storyRepository.DeleteAsync(storyForDel.IdDb.Value);
                }

                room.Stories.RemoveAll(x => x.Id == storyId);

                return true;
            });
        }



        public async Task<(string oldId, Story story)> MakeStoryCompleteAsync(string roomName, string storyId,
            string userConnectionIdRequest)
        {
            var room = await TryGetRoomAsync(roomName);
            return await MakeStoryCompleteAsync(room, storyId, userConnectionIdRequest);
        }


        public async Task<(string oldId, Story story)> MakeStoryCompleteAsync(Room room, string storyId,
            string userConnectionIdRequest)
        {
            Story res = null;
            //todo тут можно упростить тк все данные не нужны и забрать можно внутри блокировки ниже
            var voteInfo = await GetEndVoteInfo(room);
            string oldId = null;
            var sc = await UpdateIfCan(room, userConnectionIdRequest, true, async rm =>
            {
                var story = rm.Stories.FirstOrDefault(x => x.Id == storyId);
                if (story == null)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.StoryNotFound);
                }

                story.Completed = true;
                story.Vote = voteInfo?.Average ?? 0;
                story.Date = DateTime.Now;
                oldId = story.Id;

                if (story.IdDb != null)
                {
                    await _storyRepository.ChangeCompleteAsync(story.IdDb.Value, true);
                }

                if(rm.CurrentStoryId == storyId)
                {
                    rm.CurrentStoryId = string.Empty;
                }

                //rm.TotalNotActualStoriesCount++;
                res = story.Clone();


                return true;
            });

            if (sc)
            {
                return (oldId, res);
            }

            throw new SomeCustomException(ErrorConsts.SomeError);
        }


        [Obsolete]
        public async Task<List<Story>> LoadNotActualStoriesAsync(string roomName)
        {
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            var roomId = await room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Id);
            if (!roomId.sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            List<Story> res = new List<Story>();
            //комната не сохранена в бд, просто отдаем пустой список
            if (roomId.res == null)
            {
                return res;

            }


            await room.SetConcurentValueAsync(_multiThreadHelper, async rm =>
            {
                if (rm.StoredRoom.OldStoriesAreLoaded)
                {
                    return;
                }

                var notActualList = await _storyRepository.GetNotActualForRoomAsync(roomId.res.Value);
                var forAddCollection = new List<Story>();

                foreach (var newStory in notActualList)
                {
                    if (rm.StoredRoom.Stories.Any(x => x.IdDb == newStory.Id))
                        continue;
                    var typedNewStory = new Story();
                    typedNewStory.FromDbObject(newStory);
                    forAddCollection.Add(typedNewStory);
                }

                rm.StoredRoom.Stories.AddRange(forAddCollection);
                rm.StoredRoom.OldStoriesAreLoaded = true;
                res = rm.StoredRoom.Stories.Where(x => x.Completed).ToList();
            });

            return res;
        }

        /// <summary>
        /// исключает истории из текущей сессии
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="SomeCustomException"></exception>
        public async Task<List<Story>> GetNotActualStoriesAsync(string roomName, string userConnectionId, int pageNumber, int pageSize)
        {
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            var sessionStoriesIds = new List<long>();
            var roomId = await GetIfCan(room, userConnectionId, false, async rm =>
             {
                 sessionStoriesIds = rm.Stories
                     .Where(x => x.IdDb != null).Select(x => x.IdDb.Value).ToList();

                 return rm.Id;
             });

            List<Story> res = new List<Story>();
            //комната не сохранена в бд, просто отдаем пустой список
            if (roomId == null)
            {
                return res;

            }

            var notActualList = await _storyRepository
                .GetNotActualStoriesAsync(roomId.Value, pageNumber, pageSize, sessionStoriesIds);
            return notActualList.Select(x =>
            {
                var typedNewStory = new Story();
                typedNewStory.FromDbObject(x);
                return typedNewStory;
            }).ToList();

        }

        public async Task<(PlanitUser user, bool sc)> KickFromRoomAsync(string roomName, string userConnectionIdRequest,
            string userId)
        {
            var room = await TryGetRoomAsync(roomName);
            return await KickFromRoomAsync(room, userConnectionIdRequest, userId);
        }

        public async Task<(PlanitUser user, bool sc)> KickFromRoomAsync(Room room, string userConnectionIdRequest,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            PlanitUser user = null;
            var rs = await UpdateIfCan(room, userConnectionIdRequest, true, async (rm) =>
            {
                //rm.Users.RemoveAt((int)userForDelIndex);
                var us = rm.Users.FirstOrDefault(x => x.PlaningAppUserId == userId);
                if (us == null)
                {
                    user = null;
                    return true;
                }

                user = new PlanitUser(us);

                if (us.MainAppUserId == null)
                {
                    rm.Users.RemoveAll(x => x.PlaningAppUserId == userId);
                }
                else
                {
                    us.UserConnectionId = null;
                }

                return true;//Task.FromResult(true);
            });

            return (user, rs);
        }

        public async Task<(bool sc, string userId)> LeaveFromRoomAsync(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoomAsync(roomName);
            return await LeaveFromRoom(room, userConnectionIdRequest);
        }

        public async Task<(bool sc, string userId)> LeaveFromRoom(Room room, string userConnectionIdRequest)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            bool result = false;
            string userId = null;
            var rs = await UpdateIfCan(room, userConnectionIdRequest, false, async rm =>
            {
                //var admins = rm.Users.Where(x => x.IsAdmin).ToList();
                ////проверить залогинен ли пользак в мейн апе, и если залогенен то НЕ передавать админку!
                //var currentUser = admins.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);
                //if (currentUser != null)
                //{
                //    if (admins.Count < 2 && currentUser.MainAppUserId == null)
                //    {
                //        var newAdmin = rm.Users.FirstOrDefault(x => !x.IsAdmin && x.MainAppUserId != null);

                //        //if(newAdmin == null)//надо либо обновлять ui тогда через сокеты
                //        //, либо смысла не имеет, тк что бы прорасло надо обновлять страницу
                //        //а неавторизованный зайдет как новый пользак
                //        //{
                //        //    newAdmin = rm.StoredRoom.Users.FirstOrDefault(x => !x.IsAdmin);
                //        //}

                //        newAdmin?.Role.Add(Consts.Roles.Admin);
                //    }
                //}
                //else
                PlanitUser currentUser = null;
                {
                    currentUser =
                        rm.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);
                }

                if (currentUser == null)
                {
                    result = true;
                    return true;
                }

                userId = currentUser.PlaningAppUserId;
                if (currentUser.MainAppUserId == null)
                {
                    rm.Users.RemoveAll(x => x.UserConnectionId == userConnectionIdRequest);
                }
                else
                {
                    currentUser.UserConnectionId = null;
                }

                result = true;
                return true;
            });

            return (result, userId);
        }



        public async Task<bool> AllVotedAsync(Room room)
        {
            var (res, sc) = await room.GetConcurentValue(_multiThreadHelper, rm =>
                rm.StoredRoom.Users.All(x =>
                    !string.IsNullOrWhiteSpace(x.UserConnectionId)
                    && (!x.CanVote || !string.IsNullOrWhiteSpace(x.Vote))));
            if (!sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            return res;
        }

        public async Task HandleInRoomsMemoryAsync()
        {
            await HandleInRoomsMemoryAsync(true, false);
        }

        public async Task HandleInRoomsMemoryAsync(bool clearRooms = true, bool force = false)
        {
            var roomKeys = Rooms.Keys.ToList();
            foreach (var roomName in roomKeys)
            {
                var curRoom = Rooms[roomName];
                var dieDateCurRoom = await curRoom.GetConcurentValue(_multiThreadHelper,
                    rm => rm.StoredRoom.DieDate);
                if (dieDateCurRoom.res < DateTime.Now || force)//(DateTime.Now.AddHours(Consts.DefaultHourRoomAlive)))
                {
                    await curRoom.SetConcurentValueAsync(_multiThreadHelper, async rm =>
                    {
                        if (NeedSaveRoomNoLock(rm.StoredRoom))
                        {
                            await SaveRoomWithoutRightsNoLock(rm);
                        }

                        if (clearRooms)
                        {
                            Rooms.Remove(roomName, out var room);
                        }
                    });

                }
            }

        }


        public async Task<bool> ChangeRoomPasswordAsync(string roomName, string userConnectionId, string oldPassword, string newPassword)
        {
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            var sc = await UpdateIfCan(room, userConnectionId, true, async rm =>
            {
                var oldPasswordHash = string.Empty;
                if (!string.IsNullOrWhiteSpace(oldPassword))
                {
                    oldPasswordHash = _hasher.GetHash(oldPassword);
                }
                else
                {
                    oldPasswordHash = null;
                }

                if (rm.Password != oldPasswordHash// todo eql?
                    && (!string.IsNullOrEmpty(rm.Password) || !string.IsNullOrEmpty(oldPasswordHash)))
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomBadPassword);
                }

                var newPasswordHash = string.Empty;
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    newPasswordHash = _hasher.GetHash(newPassword);
                }
                else
                {
                    newPasswordHash = null;
                }

                rm.Password = newPasswordHash;
                return true;
            });

            return sc;
        }


        public async Task<bool> SetRoomCards(Room room, string userConnectionId, List<string> cards)
        {
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (cards.Count < 2 || cards.Count > 100)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomBadCountCards);
            }

            if (cards.FirstOrDefault(x => x.Length > 5) != null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomBadLengthCard);
            }

            cards = cards.Distinct().ToList();

            var sc = await UpdateIfCan(room, userConnectionId, true, async rm =>
            {
                if (rm.Status == RoomSatus.AllCanVote)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.CantVote);
                }

                rm.Cards = cards.ToList();

                return true;
            });

            var success = await ClearVotes(room);
            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            return sc;
        }

        public async Task<List<RoomShortInfo>> GetRoomsAsync(long userId)
        {
            return await _planingUserRepository.GetRoomsAsync(userId);
        }



        public async Task<string> ChangeRoomImageAsync(string roomName, long userId, IFormFile image)
        {
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            string pathImage = null;
            if (image != null)
            {
                pathImage = await _imageService.CreateUploadFileWithOutDbRecord(image);
                if (string.IsNullOrEmpty(pathImage))
                {
                    throw new SomeCustomException(ErrorConsts.FileError);
                }
            }

            var success = await UpdateIfCan(room, userId, true, async rm =>
            {
                rm.ImagePath = pathImage;
                return true;
            });

            if (success)
            {
                return pathImage;
            }

            return null;

        }


        #region private

        private async Task<bool> UpdateIfCan(Room room, string userConnectionIdRequest
            , bool isAdmin,
            Func<StoredRoom, Task<bool>> workWithRoom)
        {
            if (string.IsNullOrWhiteSpace(userConnectionIdRequest))
            {
                return false;
            }

            return await UpdateIfCan(room, (us) => us.UserConnectionId == userConnectionIdRequest, isAdmin, workWithRoom);
        }

        private async Task<bool> UpdateIfCan(Room room, long mainAppUserId
            , bool isAdmin,
            Func<StoredRoom, Task<bool>> workWithRoom)
        {

            return await UpdateIfCan(room, (us) => us.MainAppUserId == mainAppUserId, isAdmin, workWithRoom);
        }

        private async Task<bool> UpdateIfCan(Room room, Predicate<PlanitUser> userComparer
            , bool isAdmin,
        Func<StoredRoom, Task<bool>> workWithRoom)
        {
            if (room == null || workWithRoom == null)
            {
                return false;
            }

            bool result = false;
            await room.SetConcurentValueAsync(_multiThreadHelper, async rm =>
            {
                if (!rm.StoredRoom.Users
                    .Any(x => userComparer(x)
                        && ((isAdmin && x.IsAdmin) || !isAdmin)))
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.DontHaveAccess);
                }

                result = await workWithRoom(rm.StoredRoom);
            });

            ThrowBySuccess(result);


            return result;
        }

        private async Task<bool> UpdateIfCan(string roomName, string userConnectionIdRequest
            , bool isAdmin,
            Func<StoredRoom, Task<bool>> workWithRoom)
        {
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            return await UpdateIfCan(room, userConnectionIdRequest, isAdmin, workWithRoom);

        }


        private async Task<bool> UpdateUserIfCan(string roomName, string userId,
            string userConnectionIdRequest,
            Func<PlanitUser, bool> userChange)
        {
            //возможно объеденить с UpdateIfCan
            var room = await TryGetRoomAsync(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = false;
            await room.SetConcurentValue(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);
                if (user == null || !user.IsAdmin)
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.DontHaveAccess);
                }

                if (userId != user.PlaningAppUserId)
                {
                    user = rm.StoredRoom.Users.FirstOrDefault(x => x.PlaningAppUserId == userId);
                    if (user == null)
                    {
                        throw new SomeCustomException(Constants.PlanitPokerErrorConsts.PlanitUserNotFound);
                    }
                }

                result = userChange(user);

            });

            ThrowBySuccess(result);


            return result;
        }

        private async Task<T> GetIfCan<T>(Room room, string userConnectionIdRequest, bool isAdmin,
            Func<StoredRoom, Task<T>> workWithRoom)
        {
            if (room == null || string.IsNullOrWhiteSpace(userConnectionIdRequest) || workWithRoom == null)
            {
                throw new SomeCustomException(Constants.PlanitPokerErrorConsts.DontHaveAccess);
            }

            var res = await room.GetConcurentValueAsync(_multiThreadHelper, async rm =>
            {
                if (!rm.StoredRoom.Users.Any(x => x.UserConnectionId == userConnectionIdRequest
                    && ((isAdmin && x.IsAdmin) || !isAdmin)))
                {
                    throw new SomeCustomException(Constants.PlanitPokerErrorConsts.DontHaveAccess);
                }

                return await workWithRoom(rm.StoredRoom);
            });

            return res;
        }



        private void ThrowBySuccess(bool success)
        {
            if (_errorService.HasError())
            {
                throw new StopException();
            }

            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }
        }



        //users - должна быть копия! тут без локов
        private List<PlanitUser> ClearHideData(RoomSatus roomStatus, string currentUserConnectionId,
            List<PlanitUser> users)
        {
            if (users == null)
            {
                return null;
            }

            users = users.Where(x => !string.IsNullOrWhiteSpace(x.UserConnectionId)).ToList();

            users.ForEach(x =>
            {
                if (x.Vote != null)
                {
                    x.HasVote = true; //todo хорошо бы вытащить из модели
                }
            });

            if (roomStatus != RoomSatus.AllCanVote)
            {
                return users;
            }

            var user = users.FirstOrDefault(x => x.UserConnectionId == currentUserConnectionId);
            if (user == null)
            {
                return new List<PlanitUser>();
            }

            if (!user.IsAdmin)
            {
                users.ForEach(x =>
                {
                    if (x.UserConnectionId != currentUserConnectionId)
                    {
                        x.Vote = null;
                    }
                });
            }

            return users;
        }



        private PlaningRoomDal GetRoomDbObject(Room roomDb)
        {
            if (roomDb == null)
            {
                return null;
            }

            var res = new PlaningRoomDal
            {
                Name = roomDb.StoredRoom.Name,
                Password = roomDb.StoredRoom.Password,
                Id = roomDb.StoredRoom.Id ?? 0,
                Cards = JsonSerializer.Serialize(roomDb.StoredRoom.Cards), //string.Join(';', roomDb.StoredRoom.Cards),
                ImagePath = roomDb.StoredRoom.ImagePath,
            };

            return res;
        }


        /// <summary>
        /// загрузит все чего не хватает, истории только актуальные
        /// </summary>
        /// <param name="roomDb"></param>
        /// <returns></returns>
        private async Task<Room> GetRoomFromDbObject(PlaningRoomDal roomDb)
        {
            if (roomDb == null)
            {
                return null;
            }

            var storedRoom = new StoredRoom
            {
                Name = roomDb.Name,
                Password = roomDb.Password,
                Id = roomDb.Id,
                //todo наверное прям тут грузить - не очень наглядно
                Stories = (await _storyRepository.GetActualForRoomAsync(roomDb.Id)).Select(x =>
                {
                    var st = new Story() { CurrentSession = true };
                    st.FromDbObject(x);
                    return st;

                }).ToList(),
                Cards = string.IsNullOrWhiteSpace(roomDb.Cards) ? DefaultCards.Select(x => x).ToList()
                    : JsonSerializer.Deserialize<List<string>>(roomDb.Cards),
                ImagePath = roomDb.ImagePath,
                //roomDb.Cards?.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>()
            };

            await _roomRepository.LoadUsersAsync(roomDb);
            storedRoom.Users = //(await _planingUserRepository.GetForRoom(roomDb.Id))
                roomDb.Users.Select(x =>
                {
                    var st = new PlanitUser();
                    st.FromDbObject(x);
                    return st;

                }).ToList();



            return new Room(storedRoom);
        }


        /// <summary>
        /// не потокобезопасно!
        /// </summary>
        /// <param name="room"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        private async Task<List<RoomWasSavedUpdate.StoryMapping>>
            AddNewStoriesToDb(Room room, long roomId) //IEnumerable<Story> stories)
        {
            var res = new List<RoomWasSavedUpdate.StoryMapping>();
            var storiesForSave = room.StoredRoom.Stories.Where(x => x.IdDb == null)
                .Select(x => new { tmpId = x.TmpId, story = x.ToDbObject(roomId) }).ToList();
            await _storyRepository.AddAsync(storiesForSave.Select(x => x.story).ToList());
            var currentStoryIdFilled = Guid.TryParse(room.StoredRoom.CurrentStoryId, out var currentStoryId);
            foreach (var item in storiesForSave)
            {
                if (currentStoryIdFilled && currentStoryId == item.tmpId)
                {
                    room.StoredRoom.CurrentStoryId = item.story.Id.ToString();
                }

                res.Add(
                    new RoomWasSavedUpdate.StoryMapping(item.tmpId.ToString(), item.story.Id));
                var oldStory = room.StoredRoom.Stories.FirstOrDefault(x => x.TmpId == item.tmpId);
                if (oldStory != null)
                {
                    oldStory.IdDb = item.story.Id;
                }
            }

            return res;
            //room.StoredRoom.CurrentStoryId = string.Empty;
        }


        private async Task<bool> NeedSaveRoom(Room room)
        {
            var res = await room.GetConcurentValue(_multiThreadHelper,
                rm => NeedSaveRoomNoLock(rm.StoredRoom));
            return res.sc && res.res;
        }

        private bool NeedSaveRoomNoLock(StoredRoom room)
        {
            return room.Users.Any(x => x.MainAppUserId != null && x.IsAdmin);
        }


        private async Task<RoomWasSavedUpdate> SaveRoomWithoutRights(Room room)
        {
            var res = new RoomWasSavedUpdate();
            await room.SetConcurentValueAsync(_multiThreadHelper, async rm =>
            {
                await _dbHelper.ActionInTransaction(_db, async () =>
                {
                    res = await SaveRoomWithoutRightsNoLock(rm);
                });
            });

            return res;
        }

        private async Task<RoomWasSavedUpdate> SaveRoomWithoutRightsNoLock(Room room)
        {
            var res = new RoomWasSavedUpdate();
            var rm = room;
            var objForSave = GetRoomDbObject(room);
            if (objForSave == null)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }


            PlaningRoomDal roomFromDb = null;
            if (rm.StoredRoom.Id != null)
            {
                roomFromDb = await _roomRepository.GetAsync(rm.StoredRoom.Id.Value);
            }

            //вообще смысла вроде как нет, но на всякий случай пусть будет
            var roomFromDbByName = await _roomRepository.GetByNameAsync(rm.StoredRoom.Name);
            if (roomFromDbByName?.Id != roomFromDb?.Id)
            {
                throw new SomeCustomException("Имя каким то образом задублилось"); //todo
            }

            if (roomFromDb == null)
            {
                objForSave.Users.AddRange(
                    room.StoredRoom.Users.Where(x => x.MainAppUserId != null)
                        .Select(x => x.ToDbObject(objForSave.Id)).ToList());
                objForSave = await _roomRepository.AddAsync(objForSave);
                res.StoriesMapping = await AddNewStoriesToDb(room, objForSave.Id);
                res.Success = true;
                rm.StoredRoom.Id = objForSave.Id;

            }
            else
            {
                roomFromDb.Name = objForSave.Name;
                roomFromDb.Password = objForSave.Password;
                //истории и пользователей лишние удалить, новые добавить \ обновить
                await _roomRepository.LoadStoriesAsync(roomFromDb);
                await _roomRepository.LoadUsersAsync(roomFromDb);
                res.StoriesMapping = await AddNewStoriesToDb(room, roomFromDb.Id);
                foreach (var usCh in room.StoredRoom.Users)
                {
                    if (usCh.MainAppUserId == null)
                    {
                        continue;
                    }

                    var existUs = roomFromDb.Users.FirstOrDefault(x => x.MainAppUserId == usCh.MainAppUserId);
                    if (existUs == null)
                    {
                        roomFromDb.Users.Add(usCh.ToDbObject(roomFromDb.Id));
                    }
                    else
                    {
                        var tmpUs = usCh.ToDbObject(roomFromDb.Id);
                        existUs.Name = tmpUs.Name;
                        existUs.Roles = tmpUs.Roles;
                    }
                }

                await _roomRepository.UpdateAsync(roomFromDb);

                res.Success = true;
            }

            return res;
        }



        #endregion private

    }
}