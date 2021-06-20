﻿using BO.Models.PlaningPoker.DAL;
using Common.Models;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Returns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Services
{
    public class PlanitPokerService : IPlanitPokerService
    {
        private static ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();


        MultiThreadHelper _multiThreadHelper;


        //private readonly IPlanitPokerRepository _planitPokerRepository;
        private readonly IPlaningUserRepository _planingUserRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IStoryRepository _storyRepository;



        public PlanitPokerService(//IPlanitPokerRepository planitRepo,
            MultiThreadHelper multiThreadHelper,
            IPlaningUserRepository planingUserRepository,
            IRoomRepository roomRepository, IStoryRepository storyRepository)
        {
            _multiThreadHelper = multiThreadHelper;
            //_planitPokerRepository = planitRepo;

            _planingUserRepository = planingUserRepository;
            _roomRepository = roomRepository;
            _storyRepository = storyRepository;
        }

        public async Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userConnectionId)
        {
            var users = await GetAllUsers(room);
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
            return await GetAllUsersWithRight(room, userConnectionId);
        }

        public async Task<RoomInfoReturn> GetRoomInfoWithRight(string roomName, string userConnectionId)
        {
            var room = await TryGetRoom(roomName);
            return await GetRoomInfoWithRight(room, userConnectionId);
        }

        public async Task<RoomInfoReturn> GetRoomInfoWithRight(Room room, string userConnectionId)
        {
            if (string.IsNullOrWhiteSpace(userConnectionId))
            {
                return null;
            }

            if (room == null)
            {
                return null;
            }

            var roomInfo = await GetEndVoteInfo(room);//todo это можно сделать без локов тк ниже рума полностью копируется, можно создать новый метод
            var scRoom = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Clone());
            if (!scRoom.sc)
            {
                return null;
            }

            //все склонированное, работаем обычно
            var resRoom = scRoom.res;
            resRoom.Password = null;
            ClearHideData(resRoom.Status, userConnectionId, resRoom.Users);

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
            return await GetEndVoteInfo(room);

        }

        public async Task<EndVoteInfo> GetEndVoteInfo(Room room)
        {
            (var res, bool sc) = room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                if (rm.StoredRoom.Status != RoomSatus.CloseVote)
                {
                    return null;
                }

                return rm.StoredRoom.Users.Select(x => new { userId = x.PlaningAppUserId, vote = x.Vote ?? 0, hasVote = x.Vote.HasValue });
            });

            if (!sc)
            {
                //TODO
                return null;
            }

            var arrForMath = res.Where(x => x.hasVote);
            var result = new EndVoteInfo();
            if (arrForMath.Count() > 0)
            {
                result.MinVote = arrForMath.Min(x => x.vote);
                result.MaxVote = arrForMath.Max(x => x.vote);
                result.Average = arrForMath.Average(x => x.vote);
            }

            result.UsersInfo = res.Select(x => new EndVoteUserInfo() { Id = x.userId, Vote = x.vote }).ToList();
            return result;
        }










        public async Task<Room> DeleteRoom(string roomName, string userConnectionIdRequest)
        {
            var isAdmin = await UserIsAdmin(roomName, userConnectionIdRequest);
            if (!isAdmin)
            {
                return null;
            }

            if (!Rooms.Remove(roomName, out var room))
            {
                return null;
            }

            await _roomRepository.DeleteByName(roomName);
            return room;//or ?
        }

        public async Task<bool> SaveRoom(string roomName)
        {
            var room = await TryGetRoom(roomName);
            var objForSave = await GetRoomDbObject(room);
            if (objForSave == null)
            {
                return false;
            }

            var roomFromDB = _roomRepository.GetByName(roomName);

            if (roomFromDB == null)
            {
                await _roomRepository.Add(objForSave);
            }
            else
            {
                roomFromDB;
                //истории и пользователей лишних удалить, новые добавить \ обновить
                await _roomRepository.Update(objForSave);
            }

            

            objForSave = await _roomRepository.Add(objForSave);


            return (objForSave) != null;
        }


        public Task<bool> AddTimeAliveRoom(string roomName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddTimeAliveRoom(Room room)
        {
            throw new System.NotImplementedException();
        }

        public async Task<(bool sc, string oldConnectionId)> AddUserIntoRoom(string roomName, PlanitUser user)
        {
            var room = await TryGetRoom(roomName);
            return await AddUserIntoRoom(room, user);
        }

        public async Task<(bool sc, string oldConnectionId)> AddUserIntoRoom(Room room, PlanitUser user)
        {
            if (room == null || string.IsNullOrWhiteSpace(user?.UserConnectionId) || string.IsNullOrWhiteSpace(user.Name))
            {
                return (false, null);
            }
            string oldConnectionId = null;
            var success = await room.SetConcurentValueAsync<Room>(_multiThreadHelper, async rm =>
            {
                var us = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == user.UserConnectionId
                    || (user.MainAppUserId.HasValue ? x.MainAppUserId == user.MainAppUserId : false));
                if (us == null)
                {
                    if (user.MainAppUserId != null)
                    {
                        var userFromDb = await _planingUserRepository.GetByMainAppId(rm.StoredRoom.Name, user.MainAppUserId.Value);
                        if (userFromDb != null)
                        {
                            user.Role = userFromDb.Roles.Split(',').ToList();
                        }
                    }
                    rm.StoredRoom.Users.Add(user);
                }
                else
                {
                    us.Name = user.Name;
                    oldConnectionId = us.UserConnectionId;
                    us.UserConnectionId = user.UserConnectionId;
                    if (!us.MainAppUserId.HasValue)//мб это лишнее
                    {
                        us.MainAppUserId = user.MainAppUserId;
                    }
                }
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
            return await UpdateIfCan(room, userConnectionIdRequest, async rm =>
            {
                rm.Status = newStatus;
                return true;
            });


        }

        public async Task<(bool sc, string userId)> ChangeVote(Room room, string connectionUserId, int vote)
        {
            if (room == null || string.IsNullOrWhiteSpace(connectionUserId))
            {
                return (false, null);
            }

            bool result = false;
            string userId = null;
            var suc = room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                if (rm.StoredRoom.Status != RoomSatus.AllCanVote)
                {
                    return;
                }

                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == connectionUserId);
                if (user == null || !user.CanVote)
                {
                    return;
                }

                userId = user.PlaningAppUserId;
                user.Vote = vote;
                //user.HasVote = true;
                result = true;
            });

            if (!suc)
            {
                return (false, null);
            }

            return (result, userId);
        }

        public Task ClearOldRooms()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public async Task<bool> ClearVotes(Room room)
        {
            return room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                rm.StoredRoom.Users.ForEach(x =>
                {
                    x.Vote = null;
                });
            });


        }

        public async Task<Room> CreateRoomWithUser(string roomName, string password, PlanitUser user)
        {
            if (string.IsNullOrWhiteSpace(roomName) || user == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }

            var roomData = new StoredRoom(roomName, password);
            //roomData.Status = RoomSatus.AllCanVote;//todo потом убрать
            var room = new Room(roomData);
            var roomFromDb = await _roomRepository.GetByName(roomName);
            if (roomFromDb != null)
            {
                return null;//todo комната уже есть, надо бы как то сообщить об этом
            }

            var added = Rooms.TryAdd(roomName, room);
            if (added)
            {
                return room;
            }

            return null;
        }

        public async Task<List<string>> GetAdminsId(Room room)
        {
            if (room == null)
            {
                return new List<string>();
            }

            var res = room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                return rm.StoredRoom.Users.Where(x => x.IsAdmin).Select(x => x.UserConnectionId).ToList();
            });
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
        public async Task<List<PlanitUser>> GetAllUsers(Room room)
        {
            if (room == null)
            {
                return new List<PlanitUser>();
            }

            (var usersInRoom, bool suc) = room.GetConcurentValue(_multiThreadHelper,
                room => room.StoredRoom.Users.Select(x => x.Clone()).ToList());
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
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<List<PlanitUser>> GetAllUsers(string roomName)
        {


            var room = await TryGetRoom(roomName);
            return await GetAllUsers(room);
        }





        public async Task<bool> KickFromRoom(string roomName, string userConnectionIdRequest, string userId)
        {
            var room = await TryGetRoom(roomName);
            return await KickFromRoom(room, userConnectionIdRequest, userId);


        }

        public async Task<bool> KickFromRoom(Room room, string userConnectionIdRequest, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            return await UpdateIfCan(room, userConnectionIdRequest, async (rm) =>
            {


                //rm.Users.RemoveAt((int)userForDelIndex);
                rm.Users.RemoveAll(x => x.PlaningAppUserId == userId);
                return true;
            });


        }

        //public async Task<bool> RoomIsExist(string roomName)
        //{
        //    if (string.IsNullOrWhiteSpace(roomName))
        //    {
        //        return false;
        //    }

        //    return Rooms.ContainsKey(roomName);
        //}

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

        public async Task<(bool sc, string userId)> ChangeUserName(string roomName, string connectionUserId, string newUserName)
        {
            if (string.IsNullOrWhiteSpace(connectionUserId) || string.IsNullOrWhiteSpace(newUserName))
            {
                return (false, null);
            }

            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                return (false, null);
            }

            bool result = false;
            string userId = null;
            var suc = room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == connectionUserId);
                if (user == null)
                {
                    return;
                }

                user.Name = newUserName;
                userId = user.PlaningAppUserId;
                result = true;
            });

            if (!suc)
            {
                return (false, null);
            }

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

                var roomFromDb = await _roomRepository.GetByName(roomName);
                

                var dbRoom = await GetRoomFromDbObject(roomFromDb);
                if (dbRoom == null)
                {
                    return null;
                }
                var addedToCache = Rooms.TryAdd(roomName, dbRoom);//todo конечно не прям хорошо, тк грузим лишние данные а нам даже пароль может не подойти
                if (!addedToCache)
                {
                    return null;
                }
            }

            return room;
        }

        public async Task<bool> UserIsAdmin(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName, false);
            return await UserIsAdmin(room, userConnectionIdRequest);
        }

        public async Task<bool> UserIsAdmin(Room room, string userConnectionIdRequest)
        {
            if (room == null || string.IsNullOrWhiteSpace(userConnectionIdRequest))
            {
                return false;
            }

            var res = room.GetConcurentValue(_multiThreadHelper,
                rm => rm.StoredRoom.Users.Any(x => x.UserConnectionId == userConnectionIdRequest && x.IsAdmin));
            return res.sc && res.res;
        }

        //вернет true только если роль была именно добавлена
        public async Task<bool> AddNewRoleToUser(string roomName, string userId, string newRole, string userConnectionIdRequest)
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
                    return true;
                }

                return false;
            });



        }

        public async Task<bool> RemoveRoleUser(string roomName, string userId, string oldRole, string userConnectionIdRequest)
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



        public async Task<bool> AddNewStory(string roomName, string userConnectionIdRequest, Story newStory)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, async (room) =>
            {
                newStory.Id = room.StoryForAddMaxTmpId++;
                room.Stories.Add(newStory);
                return true;
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
                return true;
            });
        }

        public async Task<bool> ChangeCurrentStory(string roomName, string userConnectionIdRequest, long storyId)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, async (room) =>
            {
                var story = room.Stories.FirstOrDefault(x => x.Id == storyId);
                if (story == null)
                {
                    return false;
                }

                room.CurrentStoryId = storyId;
                return true;
            });
        }

        /// <summary>
        /// не удаляем из бд, только для текущих
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="userConnectionIdRequest"></param>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteStory(string roomName, string userConnectionIdRequest, long storyId)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, async (room) =>
            {
                if (room.CurrentStoryId == storyId)
                {
                    room.CurrentStoryId = -1;
                }



                room.Stories.RemoveAll(x => x.Id == storyId);

                return true;
            });
        }



        public async Task<(long oldId, Story story)> MakeStoryComplete(string roomName, long storyId, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            return await MakeStoryComplete(room, storyId, userConnectionIdRequest);
        }


        public async Task<(long oldId, Story story)> MakeStoryComplete(Room room, long storyId, string userConnectionIdRequest)
        {
            Story res = null;
            var voteInfo = await GetEndVoteInfo(room);//todo тут можно упростить тк все данные не нужны и забрать момжно внутри блокировки ниже
            long oldId = -1;
            var sc = await UpdateIfCan(room, userConnectionIdRequest, async rm =>
            {
                var story = rm.Stories.FirstOrDefault(x => x.Id == storyId);
                if (story == null)
                {
                    return false;

                }

                story.Completed = true;
                story.Vote = voteInfo?.Average ?? 0;
                story.Date = DateTime.Now;


                var dbRecord = story.ToDbObject();//походу тут нельзя сохранять тк еще нет id румы
                await _storyRepository.Add(dbRecord);
                oldId = story.Id;
                story.Id = dbRecord.Id;
                res = story.Clone();

                return true;
            });
            //room.SetConcurentValue(_multiThreadHelper);
            if (sc)
            {
                return (oldId, res);
            }

            return (-1, null);
        }





        //---------------------------------------------------------------------private

        private async Task<bool> UpdateIfCan(Room room, string userConnectionIdRequest, Func<StoredRoom, Task<bool>> workWithRoom)
        {
            if (room == null || string.IsNullOrWhiteSpace(userConnectionIdRequest) || workWithRoom == null)
            {
                return false;
            }

            bool result = false;
            await room.SetConcurentValueAsync<Room>(_multiThreadHelper, async rm =>
            {
                if (!rm.StoredRoom.Users.Any(x => x.UserConnectionId == userConnectionIdRequest && x.IsAdmin))
                {
                    return;
                }

                result = await workWithRoom(rm.StoredRoom);
            });

            return result;
        }

        private async Task<bool> UpdateIfCan(string roomName, string userConnectionIdRequest, Func<StoredRoom, Task<bool>> workWithRoom)
        {
            var room = await TryGetRoom(roomName);
            return await UpdateIfCan(room, userConnectionIdRequest, workWithRoom);

        }


        private async Task<bool> UpdateUserIfCan(string roomName, string userId, string userConnectionIdRequest, Func<PlanitUser, bool> userChange)
        {
            //возможно объеденить с UpdateIfCan
            var room = await TryGetRoom(roomName);

            if (room == null || string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = false;
            room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);
                if (user == null || !user.IsAdmin)
                {
                    return;
                }

                if (userId != user.PlaningAppUserId)
                {
                    user = rm.StoredRoom.Users.FirstOrDefault(x => x.PlaningAppUserId == userId);
                    if (user == null)
                    {
                        return;
                    }
                }

                result = userChange(user);

            });

            return result;
        }

        public async Task<(bool sc, string userId)> LeaveFromRoom(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            return await LeaveFromRoom(room, userConnectionIdRequest);
        }

        public async Task<(bool sc, string userId)> LeaveFromRoom(Room room, string userConnectionIdRequest)
        {
            if (room == null)
            {
                return (false, null);
            }

            bool result = false;
            string userId = null;
            room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                var admins = rm.StoredRoom.Users.Where(x => x.IsAdmin);
                //проверить залогинен ли пользак в менй апе, и если залогенен то НЕ передавать админку!
                var currentUser = admins.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest);

                if (admins.Count() < 2 && currentUser != null && currentUser.MainAppUserId == null)
                {
                    var newAdmin = rm.StoredRoom.Users.FirstOrDefault(x => !x.IsAdmin);
                    if (newAdmin != null)
                    {
                        newAdmin.Role.Add(Consts.Roles.Admin);
                    }
                }

                userId = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == userConnectionIdRequest)?.PlaningAppUserId;
                rm.StoredRoom.Users.RemoveAll(x => x.UserConnectionId == userConnectionIdRequest);

                result = true;
            });

            return (result, userId);
        }






        //-----------------------------------------------------------------------------private

        //users - должна быть копия! тут без локов
        private List<PlanitUser> ClearHideData(RoomSatus roomStatus, string currentUserConnectionId, List<PlanitUser> users)
        {
            if (users == null)
            {
                return null;
            }

            users.ForEach(x =>
            {
                if (x.Vote != null)
                {
                    x.HasVote = true;//todo хорошо бы вытащить из модели
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
                    x.Vote = null;
                });
            }

            return users;
        }



        private async Task<PlaningRoomDal> GetRoomDbObject(Room roomDb)
        {
            if (roomDb == null)
            {
                return null;
            }
            //а есть ли права на сохран
            //а авторизован ли пользак в мейн апе
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

            var storedRoom = new StoredRoom();
            storedRoom.Name = roomDb.Name;
            storedRoom.Password = roomDb.Name;

            storedRoom.Stories = (await _storyRepository.GetActualForRoom(roomDb.Name)).Select(x =>
            {
                var st = new Story();
                st.FromDbObject(x);
                return st;

            }).ToList();

            storedRoom.Users = (await _planingUserRepository.GetForRoom(roomDb.Name)).Select(x =>
            {
                var st = new PlanitUser();
                st.FromDbObject(x);
                return st;

            }).ToList();



            return new Room(storedRoom);
        }


    }
}