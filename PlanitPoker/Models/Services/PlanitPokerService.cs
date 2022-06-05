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

namespace PlanitPoker.Models.Services
{
    public sealed class PlanitPokerService : IPlanitPokerService
    {
        private static readonly ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();

        private readonly MenuDbContext _db;


        private readonly MultiThreadHelper _multiThreadHelper;

        private readonly IRoomRepository _roomRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IErrorService _errorService;
        private readonly IErrorContainer _errorContainer;


        private readonly DBHelper _dbHelper;



        public PlanitPokerService(
            MultiThreadHelper multiThreadHelper,
            IRoomRepository roomRepository, IStoryRepository storyRepository
            , IErrorService errorService, IErrorContainer errorContainer,
            DBHelper dbHelper, MenuDbContext db
        )
        {
            _multiThreadHelper = multiThreadHelper;
            _roomRepository = roomRepository;
            _storyRepository = storyRepository;

            _errorService = errorService;
            _errorContainer = errorContainer;
            _dbHelper = dbHelper;
            _db = db;
        }

        public List<PlanitUser> GetAllUsersWithRight(Room room, string userConnectionId)
        {
            var users = GetAllUsers(room);
            if (users == null || users.Count == 0)
            {
                return new List<PlanitUser>();
            }

            var roomStatusR = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Status);
            if (!roomStatusR.sc)
            {
                return new List<PlanitUser>();
            }

            return ClearHideData(roomStatusR.res, userConnectionId, users);
        }

        public async Task<List<PlanitUser>> GetAllUsersWithRight(string roomName, string userConnectionId)
        {
            var room = await TryGetRoom(roomName);
            return GetAllUsersWithRight(room, userConnectionId);
        }

        public async Task<RoomInfoReturn> GetRoomInfoWithRight(string roomName, string userConnectionId)
        {
            var room = await TryGetRoom(roomName);
            return GetRoomInfoWithRight(room, userConnectionId);
        }

        public RoomInfoReturn GetRoomInfoWithRight(Room room, string userConnectionId)
        {
            if (string.IsNullOrWhiteSpace(userConnectionId))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            //todo это можно сделать без локов тк ниже рума полностью копируется, можно создать новый метод
            var roomInfo = GetEndVoteInfo(room);
            var scRoom = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Clone());
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


        public async Task<EndVoteInfo> GetEndVoteInfo(string roomName)
        {
            var room = await TryGetRoom(roomName);
            return GetEndVoteInfo(room);

        }

        public EndVoteInfo GetEndVoteInfo(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            (var res, bool sc) = room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                if (rm.StoredRoom.Status != RoomSatus.CloseVote)
                {
                    return null;
                }

                return rm.StoredRoom.Users.Where(x => !string.IsNullOrWhiteSpace(x.UserConnectionId))
                    .Select(x =>
                        new {userId = x.PlaningAppUserId, vote = x.Vote, hasVote = !string.IsNullOrWhiteSpace(x.Vote)})
                    .ToList();
            });

            if (!sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            if (res == null)
            {
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

            result.UsersInfo = res.Select(x => new EndVoteUserInfo() {Id = x.userId, Vote = x.vote}).ToList();
            return result;
        }










        public async Task<Room> DeleteRoom(string roomName, string userConnectionIdRequest)
        {
            var isAdmin = await UserIsAdmin(roomName, userConnectionIdRequest);
            if (!isAdmin)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.DontHaveAccess);

            }

            if (!Rooms.Remove(roomName, out var room))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            await _roomRepository.DeleteByNameAsync(roomName);
            return room;
        }

        public async Task<bool> SaveRoom(string roomName, string userConnectionIdRequest)
        {
            //только из памяти берем
            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                var currentUserFromRoom = room.StoredRoom.Users.FirstOrDefault(x =>
                    x.UserConnectionId == userConnectionIdRequest && x.IsAdmin && x.MainAppUserId != null);
                if (currentUserFromRoom == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.DontHaveAccess);
                }

                return true;
            });

            return await SaveRoomWithoutRights(room);
        }


        public async Task<DateTime> AddTimeAliveRoom(string roomName)
        {
            var room = await TryGetRoom(roomName);
            return AddTimeAliveRoom(room);
        }

        public DateTime AddTimeAliveRoom(Room room)
        {
            var res = DateTime.MinValue;
            room.SetConcurentValue(_multiThreadHelper,
                rm =>
                {
                    res = DateTime.Now.AddHours(Consts.DefaultHourRoomAlive);
                    rm.StoredRoom.DieDate = res;
                });

            return res;
        }

        /// <summary>
        /// то надо сохраняет, что надо чистит
        /// </summary>
        /// <returns></returns>
        public async Task ClearOldRooms()
        {
            var roomKeys = Rooms.Keys.ToList();
            foreach (var roomName in roomKeys)
            {
                var curRoom = Rooms[roomName];
                var dieDateCurRoom = curRoom.GetConcurentValue(_multiThreadHelper,
                    rm => rm.StoredRoom.DieDate);
                if (dieDateCurRoom.res < DateTime.Now)//(DateTime.Now.AddHours(Consts.DefaultHourRoomAlive)))
                {
                    await curRoom.SetConcurentValueAsync(_multiThreadHelper,async rm => {
                        if (NeedSaveRoomNoLock(rm.StoredRoom))
                        {
                            await SaveRoomWithoutRightsNoLock(rm);

                        }

                        Rooms.Remove(roomName, out var room);
                    });
                    
                }
            }
        }

        public async Task<(bool sc, string oldConnectionId)> AddUserIntoRoom(string roomName, PlanitUser user)
        {
            var room = await TryGetRoom(roomName);
            return await AddUserIntoRoom(room, user);
        }

        public async Task<(bool sc, string oldConnectionId)> AddUserIntoRoom(Room room, PlanitUser user)
        {

            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(user?.UserConnectionId) ||
                string.IsNullOrWhiteSpace(user.Name))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            string oldConnectionId = null;
            var success = await room.SetConcurentValueAsync(_multiThreadHelper, rm =>
            {
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
                }

                return Task.CompletedTask;
            });

            return (success, oldConnectionId);
            //return true;
        }

        public async Task<bool> ChangeStatusIfCan(string roomName, string userConnectionIdRequest, RoomSatus newStatus)
        {
            var room = await TryGetRoom(roomName);
            return await ChangeStatusIfCan(room, userConnectionIdRequest, newStatus);
        }

        public async Task<bool> ChangeStatusIfCan(Room room, string userConnectionIdRequest, RoomSatus newStatus)
        {
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(userConnectionIdRequest))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            return await UpdateIfCan(room, userConnectionIdRequest, rm =>
            {
                rm.Status = newStatus;
                return Task.FromResult(true);
            });
        }

        public (bool sc, string userId) ChangeVote(Room room, string connectionUserId, string vote)
        {
            if (room == null)
            {

                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(connectionUserId))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            bool result = false;
            string userId = null;
            var suc = room.SetConcurentValue(_multiThreadHelper, rm =>
            {
                if (rm.StoredRoom.Status != RoomSatus.AllCanVote)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.CantVote);
                }

                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == connectionUserId);
                if (user == null || !user.CanVote)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.CantVote);
                }

                userId = user.PlaningAppUserId;
                user.Vote = vote;
                result = true;
            });

            ThrowBySuccess(suc);

            return (result, userId);
        }



        public bool ClearVotes(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            return room.SetConcurentValue(_multiThreadHelper,
                rm => { rm.StoredRoom.Users.ForEach(x => { x.Vote = null; }); });
        }

        public async Task<Room> CreateRoomWithUser(string roomName, string password, PlanitUser user)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNameIsEmpty);
            }

            if (user == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }

            if (Rooms.ContainsKey(roomName))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomAlreadyExist);
            }

            var roomData = new StoredRoom(roomName, password);
            var room = new Room(roomData);
            var roomFromDb = await _roomRepository.ExistAsync(roomName);
            if (roomFromDb) // != null
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomAlreadyExist);
            }

            var added = Rooms.TryAdd(roomName, room);
            if (added)
            {
                return room;
            }

            throw new SomeCustomException(Consts.PlanitPokerErrorConsts.SomeErrorWithRoomCreating);
        }

        public async Task<List<string>> GetAdminsId(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            var res = room.GetConcurentValue(_multiThreadHelper,
                rm => rm.StoredRoom.Users.Where(x => x.IsAdmin).Select(x => x.UserConnectionId).ToList());
            if (!res.sc)
            {
                return new List<string>();
            }

            return res.res;
        }

        public async Task<List<string>> GetAdminsId(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return new List<string>();
            }

            var room = await TryGetRoom(roomName);
            return await GetAdminsId(room);
        }

        /// <summary>
        /// возвращает копию пользователей
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public List<PlanitUser> GetAllUsers(Room room)
        {
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }


            (var usersInRoom, bool suc) = room.GetConcurentValue(_multiThreadHelper,
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
        public async Task<List<PlanitUser>> GetAllUsers(string roomName)
        {
            var room = await TryGetRoom(roomName);
            return GetAllUsers(room);
        }



        public async Task<(PlanitUser user, bool sc)> KickFromRoom(string roomName, string userConnectionIdRequest,
            string userId)
        {
            var room = await TryGetRoom(roomName);
            return await KickFromRoom(room, userConnectionIdRequest, userId);


        }

        public async Task<(PlanitUser user, bool sc)> KickFromRoom(Room room, string userConnectionIdRequest,
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            PlanitUser user = null;
            var rs = await UpdateIfCan(room, userConnectionIdRequest, (rm) =>
            {
                //rm.Users.RemoveAt((int)userForDelIndex);
                user = rm.Users.FirstOrDefault(x => x.PlaningAppUserId == userId);
                rm.Users.RemoveAll(x => x.PlaningAppUserId == userId);
                return Task.FromResult(true);
            });

            return (user, rs);
        }

        /// <summary>
        /// тянет еще и из бд
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Room> TryGetRoom(string roomName, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }

            var room = await TryGetRoom(roomName, false);
            if (room == null)
            {
                return null;
            }

            var storedPs = room.GetConcurentValue(_multiThreadHelper, (rm) => rm.StoredRoom.Password);
            if (!storedPs.sc)
            {
                return null;
            }

            if (storedPs.res == password)
            {
                return room;
            }

            return null;
        }

        public async Task<(bool sc, string userId)> ChangeUserName(string roomName, string connectionUserId,
            string newUserName)
        {
            if (string.IsNullOrWhiteSpace(connectionUserId) || string.IsNullOrWhiteSpace(newUserName))
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
            }

            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            bool result = false;
            string userId = null;
            var suc = room.SetConcurentValue(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == connectionUserId);
                if (user == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
                }

                user.Name = newUserName;
                userId = user.PlaningAppUserId;
                result = true;
            });


            ThrowBySuccess(suc);


            return (result, userId);
        }

        public async Task<Room> TryGetRoom(string roomName, bool cacheOnly = true)
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

                //todo конечно не прям хорошо, тк грузим лишние данные а нам даже пароль может не подойти
                var addedToCache = Rooms.TryAdd(roomName, dbRoom);
                if (!addedToCache)
                {
                    return null;
                }

                room = dbRoom;
            }

            return room;
        }

        public async Task<bool> UserIsAdmin(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            return UserIsAdmin(room, userConnectionIdRequest);
        }

        public bool UserIsAdmin(Room room, string userConnectionIdRequest)
        {
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(userConnectionIdRequest))
            {
                return false;
            }

            var res = room.GetConcurentValue(_multiThreadHelper,
                rm => rm.StoredRoom.Users.Any(x => x.UserConnectionId == userConnectionIdRequest && x.IsAdmin));
            return res.sc && res.res;
        }

        //вернет true только если роль была именно добавлена
        public async Task<bool> AddNewRoleToUser(string roomName, string userId, string newRole,
            string userConnectionIdRequest)
        {
            if (!Consts.Roles.IsValideRole(newRole))
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

        public async Task<bool> RemoveRoleUser(string roomName, string userId, string oldRole,
            string userConnectionIdRequest)
        {
            if (!Consts.Roles.IsValideRole(oldRole))
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



        public async Task StartVote(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            var success =
                await ChangeStatusIfCan(room, userConnectionIdRequest, Enums.RoomSatus.AllCanVote);
            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            success = ClearVotes(room);
            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }
        }

        public async Task<EndVoteInfo> EndVote(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            var success = await ChangeStatusIfCan(room, userConnectionIdRequest, RoomSatus.CloseVote);
            if (!success)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            var result = GetEndVoteInfo(room);
            return result;

        }


        public async Task<bool> AddNewStory(string roomName, string userConnectionIdRequest, Story newStory)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, (room) =>
            {
                newStory.TmpId = Guid.NewGuid();
                room.Stories.Add(newStory);
                return Task.FromResult(true);
            });

        }

        public async Task<bool> ChangeStory(string roomName, string userConnectionIdRequest, Story newData)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, async (room) =>
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

        public async Task<bool> ChangeCurrentStory(string roomName, string userConnectionIdRequest, string storyId)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, (room) =>
            {
                var story = room.Stories.FirstOrDefault(x => x.Id == storyId);
                if (story == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.StoryNotFound);
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
        public async Task<bool> DeleteStory(string roomName, string userConnectionIdRequest, string storyId)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, async (room) =>
            {
                if (room.CurrentStoryId == storyId)
                {
                    room.CurrentStoryId = "";
                }

                var storyForDel = room.Stories.FirstOrDefault(x => x.Id == storyId);
                if (storyForDel == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.StoryNotFound);
                }

                if (storyForDel.Completed)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.StoryBadStatus);
                }

                if (storyForDel.IdDb != null)
                {
                    await _storyRepository.DeleteAsync(storyForDel.IdDb.Value);
                }

                room.Stories.RemoveAll(x => x.Id == storyId);

                return true;
            });
        }



        public async Task<(string oldId, Story story)> MakeStoryComplete(string roomName, string storyId,
            string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            return await MakeStoryComplete(room, storyId, userConnectionIdRequest);
        }


        public async Task<(string oldId, Story story)> MakeStoryComplete(Room room, string storyId,
            string userConnectionIdRequest)
        {
            Story res = null;
            //todo тут можно упростить тк все данные не нужны и забрать можно внутри блокировки ниже
            var voteInfo = GetEndVoteInfo(room);
            string oldId = null;
            var sc = await UpdateIfCan(room, userConnectionIdRequest, rm =>
            {
                var story = rm.Stories.FirstOrDefault(x => x.Id == storyId);
                if (story == null)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.StoryNotFound);
                }

                story.Completed = true;
                story.Vote = voteInfo?.Average ?? 0;
                story.Date = DateTime.Now;
                oldId = story.Id;

                res = story.Clone();

                return Task.FromResult(true);
            });

            if (sc)
            {
                return (oldId, res);
            }

            throw new SomeCustomException(ErrorConsts.SomeError);
        }


        public async Task<List<Story>> LoadNotActualStories(string roomName)
        {
            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            var roomId = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Id);
            if (!roomId.sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            if (roomId.res == null)
            {
                return new List<Story>();

            }

            List<Story> res = new List<Story>();

            await room.SetConcurentValueAsync(_multiThreadHelper, async rm =>
            {
                if (rm.StoredRoom.OldStoriesAreLoaded)
                {
                    return;
                }

                var notActualList = await _storyRepository.GetNotActualForRoom(roomId.res.Value);
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

        public async Task<(bool sc, string userId)> LeaveFromRoom(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            return LeaveFromRoom(room, userConnectionIdRequest);
        }

        public (bool sc, string userId) LeaveFromRoom(Room room, string userConnectionIdRequest)
        {
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            bool result = false;
            string userId = null;
            room.SetConcurentValue(_multiThreadHelper, rm =>
            {
                var admins = rm.StoredRoom.Users.Where(x => x.IsAdmin).ToList();
                //проверить залогинен ли пользак в мейн апе, и если залогенен то НЕ передавать админку!
                var currentUser = admins.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);
                if (currentUser != null)
                {
                    if (admins.Count < 2 && currentUser.MainAppUserId == null)
                    {
                        var newAdmin = rm.StoredRoom.Users.FirstOrDefault(x => !x.IsAdmin);
                        newAdmin?.Role.Add(Consts.Roles.Admin);
                    }
                }
                else
                {
                    currentUser =
                        rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);
                }

                if (currentUser == null)
                {
                    result = true;
                    return;
                }

                userId = currentUser.PlaningAppUserId;
                if (currentUser.MainAppUserId == null)
                {
                    rm.StoredRoom.Users.RemoveAll(x => x.UserConnectionId == userConnectionIdRequest);
                }
                else
                {
                    currentUser.UserConnectionId = null;
                }

                result = true;
            });

            return (result, userId);
        }



        public Task<bool> AllVoted(Room room)
        {
            var (res, sc) = room.GetConcurentValue(_multiThreadHelper, rm =>
                rm.StoredRoom.Users.All(x =>
                    !string.IsNullOrWhiteSpace(x.UserConnectionId)
                    && (!x.CanVote || !string.IsNullOrWhiteSpace(x.Vote))));
            if (!sc)
            {
                throw new SomeCustomException(ErrorConsts.SomeError);
            }

            return Task.FromResult(res);
        }


        public async Task HandleInRoomsMemoryAsync()
        {
            //var now = DateTime.Now;
            //var rooms = Rooms.Select(x => x.Value.GetConcurentValue(_multiThreadHelper, r => r.StoredRoom.DieDate).res < now);
            //foreach (var room in rooms)
            //{

            //}
            await ClearOldRooms();
        }




        #region private

        private async Task<bool> UpdateIfCan(Room room, string userConnectionIdRequest,
            Func<StoredRoom, Task<bool>> workWithRoom)
        {
            if (room == null || string.IsNullOrWhiteSpace(userConnectionIdRequest) || workWithRoom == null)
            {
                return false;
            }

            bool result = false;
            await room.SetConcurentValueAsync(_multiThreadHelper, async rm =>
            {
                if (!rm.StoredRoom.Users.Any(x => x.UserConnectionId == userConnectionIdRequest && x.IsAdmin))
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.DontHaveAccess);
                }

                result = await workWithRoom(rm.StoredRoom);
            });

            ThrowBySuccess(result);


            return result;
        }

        private async Task<bool> UpdateIfCan(string roomName, string userConnectionIdRequest,
            Func<StoredRoom, Task<bool>> workWithRoom)
        {
            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            return await UpdateIfCan(room, userConnectionIdRequest, workWithRoom);

        }


        private async Task<bool> UpdateUserIfCan(string roomName, string userId, string userConnectionIdRequest,
            Func<PlanitUser, bool> userChange)
        {
            //возможно объеденить с UpdateIfCan
            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                throw new SomeCustomException(Consts.PlanitPokerErrorConsts.RoomNotFound);
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = false;
            room.SetConcurentValue(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);
                if (user == null || !user.IsAdmin)
                {
                    throw new SomeCustomException(Consts.PlanitPokerErrorConsts.DontHaveAccess);
                }

                if (userId != user.PlaningAppUserId)
                {
                    user = rm.StoredRoom.Users.FirstOrDefault(x => x.PlaningAppUserId == userId);
                    if (user == null)
                    {
                        throw new SomeCustomException(Consts.PlanitPokerErrorConsts.PlanitUserNotFound);
                    }
                }

                result = userChange(user);

            });

            ThrowBySuccess(result);


            return result;
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
                Id = roomDb.StoredRoom.Id ?? 0
            };

            return res;
        }

        private PlaningRoomDal GetRoomDbObject(StoredRoom roomDb)
        {
            if (roomDb == null)
            {
                return null;
            }

            var res = new PlaningRoomDal
            {
                Name = roomDb.Name,
                Password = roomDb.Password,
                Id = roomDb.Id ?? 0
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

                Stories = (await _storyRepository.GetActualForRoom(roomDb.Id)).Select(x =>
                {
                    var st = new Story();
                    st.FromDbObject(x);
                    return st;

                }).ToList()
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
        private async Task AddNewStoriesToDb(Room room, long roomId) //IEnumerable<Story> stories)
        {
            var storiesForSave = room.StoredRoom.Stories.Where(x => x.IdDb == null)
                .Select(x => new {tmpId = x.TmpId, story = x.ToDbObject(roomId)}).ToList();
            await _storyRepository.AddAsync(storiesForSave.Select(x => x.story).ToList());
            foreach (var item in storiesForSave)
            {
                var oldStory = room.StoredRoom.Stories.FirstOrDefault(x => x.TmpId == item.tmpId);
                if (oldStory != null)
                {
                    oldStory.IdDb = item.story.Id;
                }
            }
        }


        private bool NeedSaveRoom(Room room)
        {
            var res = room.GetConcurentValue(_multiThreadHelper,
                rm => NeedSaveRoomNoLock(rm.StoredRoom));
            return res.sc && res.res;
        }

        private bool NeedSaveRoomNoLock(StoredRoom room)
        {
            return room.Users.Any(x => x.MainAppUserId != null && x.IsAdmin);
        }


        private async Task<bool> SaveRoomWithoutRights(Room room)
        {
            var res = false;
            await room.SetConcurentValueAsync(_multiThreadHelper, async rm =>
            {
                await _dbHelper.ActionInTransaction(_db, async () => {
                    res = await SaveRoomWithoutRightsNoLock(rm);
                });
            });

            return res;
        }

        private async Task<bool> SaveRoomWithoutRightsNoLock(Room room)
        {
            var success = false;
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
                throw new SomeCustomException("имя каким то образом задублилось"); //todo
            }

            if (roomFromDb == null)
            {
                objForSave.Users.AddRange(
                    room.StoredRoom.Users.Where(x => x.MainAppUserId != null)
                        .Select(x => x.ToDbObject(objForSave.Id)).ToList());
                objForSave = await _roomRepository.AddAsync(objForSave);
                await AddNewStoriesToDb(room, objForSave.Id);
                success = true;
            }
            else
            {
                roomFromDb.Name = objForSave.Name;
                roomFromDb.Password = objForSave.Password;
                //истории и пользователей лишние удалить, новые добавить \ обновить
                await _roomRepository.LoadStoriesAsync(roomFromDb);
                await _roomRepository.LoadUsersAsync(roomFromDb);
                await AddNewStoriesToDb(room, roomFromDb.Id);
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

                success = true;
            }

            return success;
        }

        #endregion private

    }
}
