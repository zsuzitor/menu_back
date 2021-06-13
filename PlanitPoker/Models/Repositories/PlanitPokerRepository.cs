using Common.Models;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories
{
    public class PlanitPokerRepository : IPlanitPokerRepository
    {
        private static ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();

        private readonly MultiThreadHelper _multiThreadHelper;

        public PlanitPokerRepository(MultiThreadHelper multiThreadHelper)
        {
            _multiThreadHelper = multiThreadHelper;
        }

        //public Task<bool> AddAdmin(string roomName, string userId)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Task<bool> AddAdmin(Room room, string userId)
        //{
        //    throw new System.NotImplementedException();
        //}

        public Task<bool> AddTimeAliveRoom(string roomName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddTimeAliveRoom(Room room)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> AddUserIntoRoom(string roomName, PlanitUser user)
        {
            var room = await TryGetRoom(roomName);
            return await AddUserIntoRoom(room, user);
        }

        public async Task<bool> AddUserIntoRoom(Room room, PlanitUser user)
        {
            if (room == null || string.IsNullOrWhiteSpace(user?.UserConnectionId) || string.IsNullOrWhiteSpace(user.Name))
            {
                return false;
            }

            room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                var us = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == user.UserConnectionId
                    || (user.MainAppUserId.HasValue ? x.MainAppUserId == user.MainAppUserId : false));
                if (us == null)
                {
                    rm.StoredRoom.Users.Add(user);
                }
                else
                {
                    us.Name = user.Name;
                }
            });

            return true;
        }

        public async Task<bool> ChangeStatusIfCan(string roomName, string userConnectionIdRequest, RoomSatus newStatus)
        {
            var room = await TryGetRoom(roomName);
            return await ChangeStatusIfCan(room, userConnectionIdRequest, newStatus);
        }

        public async Task<bool> ChangeStatusIfCan(Room room, string userConnectionIdRequest, RoomSatus newStatus)
        {
            return await UpdateIfCan(room, userConnectionIdRequest, rm =>
            {
                rm.Status = newStatus;
                return true;
            });
            //if (room == null || string.IsNullOrWhiteSpace(userId))
            //{
            //    return false;
            //}

            //bool result = false;
            //room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            //{
            //    var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == userId);
            //    if (user == null)
            //    {
            //        return;
            //    }
            //    if (!user.IsAdmin)
            //    {
            //        return;
            //    }
            //    rm.StoredRoom.Status = newStatus;
            //    result = true;
            //});

            //return result;
        }

        public async Task<bool> ChangeVote(Room room, string connectionUserId, int vote)
        {
            if (room == null || string.IsNullOrWhiteSpace(connectionUserId))
            {
                return false;
            }

            bool result = false;
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


                user.Vote = vote;
                result = true;
            });

            if (!suc)
            {
                return false;
            }

            return result;
        }

        public Task ClearOldRooms()
        {
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

            return await UpdateIfCan(room, userConnectionIdRequest, (rm) =>
            {
                //var userForDelIndex = rm.Users.Select((us, index) => new { us, index })
                //    .FirstOrDefault(x => x.us.UserIdentifier == userId)?.index;
                //if (userForDelIndex == null)
                //{
                //    return false;
                //}

                //rm.Users.RemoveAt((int)userForDelIndex);
                rm.Users.RemoveAll(x => x.PlaningAppUserId == userId);
                return true;
            });

            //if (room == null || string.IsNullOrWhiteSpace(userIdRequest) || string.IsNullOrWhiteSpace(userId))
            //{
            //    return false;
            //}

            //bool result = false;
            //room.SetConcurentValue<Room>(_multiThreadHelper, (rm) =>
            //{
            //    var usRequest = rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == userIdRequest);
            //    if (!usRequest.IsAdmin)
            //    {
            //        return;
            //    }
            //    //x => x.UserIdentifier == userId
            //    //rm.StoredRoom.Users.IndexOf(,);
            //    var userForDelIndex = rm.StoredRoom.Users.Select((us, index) => new { us, index })
            //        .FirstOrDefault(x => x.us.UserIdentifier == userId)?.index;
            //    if (userForDelIndex == null)
            //    {
            //        return;
            //    }

            //    rm.StoredRoom.Users.RemoveAt((int)userForDelIndex);
            //    result = true;
            //});

            //return result;
        }

        public async Task<bool> RoomIsExist(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return false;
            }

            return Rooms.ContainsKey(roomName);
        }

        public async Task<Room> TryGetRoom(string roomName, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                password = null;
            }

            var room = await TryGetRoom(roomName);
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

        public async Task<bool> ChangeUserName(string roomName, string connectionUserId, string newUserName)
        {
            if (string.IsNullOrWhiteSpace(connectionUserId) || string.IsNullOrWhiteSpace(newUserName))
            {
                return false;
            }

            var room = await TryGetRoom(roomName);
            if (room == null)
            {
                return false;
            }

            bool result = false;
            var suc = room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserConnectionId == connectionUserId);
                if (user == null)
                {
                    return;
                }

                user.Name = newUserName;
                result = true;
            });

            if (!suc)
            {
                return false;
            }

            return result;
        }

        public async Task<Room> TryGetRoom(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return null;
            }

            var exist = Rooms.TryGetValue(roomName, out var room);
            if (!exist)
            {
                return null;
            }

            return room;
        }

        public async Task<bool> UserIsAdmin(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
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
        public async Task<bool> AddNewStatusToUser(string roomName, string userId, string newRole, string userConnectionIdRequest)
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

        public async Task<bool> RemoveStatusUser(string roomName, string userId, string oldRole, string userConnectionIdRequest)
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
            return await UpdateIfCan(roomName, userConnectionIdRequest, (room) =>
            {
                newStory.Id = room.StoryForAddMaxTmpId++;
                room.Stories.Add(newStory);
                return true;
            });

        }

        public async Task<bool> ChangeStory(string roomName, string userConnectionIdRequest, Story newData)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, (room) =>
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
            return await UpdateIfCan(roomName, userConnectionIdRequest, (room) =>
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

        public async Task<bool> DeleteStory(string roomName, string userConnectionIdRequest, long storyId)
        {
            return await UpdateIfCan(roomName, userConnectionIdRequest, (room) =>
            {
                if (room.CurrentStoryId == storyId)
                {
                    room.CurrentStoryId = -1;
                }

                //var story = room.Stories.FirstOrDefault(x => x.Id == storyId);
                //if (story == null)
                //{
                //    return false;
                //}

                room.Stories.RemoveAll(x => x.Id == storyId);

                return true;
            });
        }



        public async Task<bool> MakeStoryComplete(string roomName, long storyId, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            return await MakeStoryComplete(room, storyId, userConnectionIdRequest);
        }


        public async Task<bool> MakeStoryComplete(Room room, long storyId, string userConnectionIdRequest)
        {
            return await UpdateIfCan(room, userConnectionIdRequest, rm =>
              {
                  var story = rm.Stories.FirstOrDefault(x => x.Id == storyId);
                  if (story == null)
                  {
                      return false;

                  }
                  //todo возможно на +-этом моменте надо писать в бд и обновлять сразу id стори, 
                  //или может отдавать юзеру ответ и параллельно это делать
                  // или ждать уже полное сохранение комнаты
                  story.Completed = true;
                  story.Date = DateTime.Now;
                  return true;
              });
            //room.SetConcurentValue(_multiThreadHelper);
        }



        //---------------------------------------------------------------------private

        private async Task<bool> UpdateIfCan(Room room, string userConnectionIdRequest, Func<StoredRoom, bool> workWithRoom)
        {
            if (room == null || string.IsNullOrWhiteSpace(userConnectionIdRequest) || workWithRoom == null)
            {
                return false;
            }

            bool result = false;
            room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                if (!rm.StoredRoom.Users.Any(x => x.UserConnectionId == userConnectionIdRequest && x.IsAdmin))
                {
                    return;
                }

                result = workWithRoom(rm.StoredRoom);
            });

            return result;
        }

        private async Task<bool> UpdateIfCan(string roomName, string userConnectionIdRequest, Func<StoredRoom, bool> workWithRoom)
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

        public async Task<bool> LeaveFromRoom(string roomName, string userConnectionIdRequest)
        {
            var room = await TryGetRoom(roomName);
            return await LeaveFromRoom(room, userConnectionIdRequest);
        }

        public async Task<bool> LeaveFromRoom(Room room, string userConnectionIdRequest)
        {
            if (room == null)
            {
                return false;
            }

            bool result = false;
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

                rm.StoredRoom.Users.RemoveAll(x => x.UserConnectionId == userConnectionIdRequest);
                result = true;
            });

            return result;
        }
    }
}
