using Common.Models;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
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

        public Task<bool> AddAdmin(string roomName, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddAdmin(Room room, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddTimeAliveRoom(string roomname)
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
            if (room == null || string.IsNullOrWhiteSpace(user?.UserIdentifier) || string.IsNullOrWhiteSpace(user.Name))
            {
                return false;
            }

            room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                var us = rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == user.UserIdentifier);
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

        public async Task<bool> ChangeStatusIfCan(string roomName, string userId, RoomSatus newStatus)
        {
            var room = await TryGetRoom(roomName);
            return await ChangeStatusIfCan(room, userId, newStatus);
        }

        public async Task<bool> ChangeStatusIfCan(Room room, string userId, RoomSatus newStatus)
        {
            if (room == null || string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = false;
            room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == userId);
                if (user == null)
                {
                    return;
                }
                if (!user.IsAdmin)
                {
                    return;
                }
                rm.StoredRoom.Status = newStatus;
                result = true;
            });

            return result;
        }

        public async Task<bool> ChangeVote(Room room, string userId, int vote)
        {
            if (room == null || string.IsNullOrWhiteSpace(userId))
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

                var user = rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == userId);
                if (user == null)
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

        public async Task<Room> CreateRoomWithUser(string roomname, string password, PlanitUser user)
        {
            if (string.IsNullOrWhiteSpace(roomname) || string.IsNullOrWhiteSpace(password) || user == null)
            {
                return null;
            }

            var roomData = new StoredRoom(roomname, password);
            //roomData.Status = RoomSatus.AllCanVote;//todo потом убрать
            var room = new Room(roomData);
            var added = Rooms.TryAdd(roomname, room);
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
                return rm.StoredRoom.Users.Where(x => x.IsAdmin).Select(x => x.UserIdentifier).ToList();
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

        /// <summary>
        /// возвращает копию пользователей
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<List<PlanitUser>> GetAllUsersWithRight(string roomName, string userId)
        {


            var room = await TryGetRoom(roomName);
            return await GetAllUsersWithRight(room, userId);
        }

        /// <summary>
        /// возвращает копию пользователей
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userId)
        {
            if (room == null || string.IsNullOrWhiteSpace(userId))
            {
                return new List<PlanitUser>();
            }
            var allUsers = await GetAllUsers(room);
            var roomStatusR = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Status);
            if (!roomStatusR.sc)
            {
                return new List<PlanitUser>();
            }

            return ClearHideData(roomStatusR.res, userId, allUsers);



        }

        /// <summary>
        /// режет инфу в зависимости от того есть ли к ней доступ
        /// </summary>
        /// <param name="roomname"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<StoredRoom> GetRoomInfo(string roomname, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return null;
            }

            var room = await TryGetRoom(roomname);
            if (room == null)
            {
                return null;
            }

            var res = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Clone());
            if (!res.sc)
            {
                return null;
            }

            //все склонированное, работаем обычно
            var resRoom = res.res;
            resRoom.Password = null;
            ClearHideData(resRoom.Status, currentUserId, resRoom.Users);


            return resRoom;
        }

        public async Task<bool> KickFromRoom(string roomName, string userIdRequest, string userId)
        {
            var room = await TryGetRoom(roomName);
            return await KickFromRoom(room, userIdRequest, userId);


        }

        public async Task<bool> KickFromRoom(Room room, string userIdRequest, string userId)
        {
            if (room == null || string.IsNullOrWhiteSpace(userIdRequest) || string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = false;
            room.SetConcurentValue<Room>(_multiThreadHelper, (rm) =>
            {
                var usRequest = rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == userIdRequest);
                if (!usRequest.IsAdmin)
                {
                    return;
                }
                //x => x.UserIdentifier == userId
                //rm.StoredRoom.Users.IndexOf(,);
                var userForDelIndex = rm.StoredRoom.Users.Select((us, index) => new { us, index })
                    .FirstOrDefault(x => x.us.UserIdentifier == userId)?.index;
                if (userForDelIndex == null)
                {
                    return;
                }

                rm.StoredRoom.Users.RemoveAt((int)userForDelIndex);
                result = true;
            });

            return result;
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
                return null;
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

        public async Task<bool> UserIsAdmin(string roomName, string userId)
        {
            var room = await TryGetRoom(roomName);
            return await UserIsAdmin(room, userId);
        }

        public async Task<bool> UserIsAdmin(Room room, string userId)
        {
            if (room == null || string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            var res = room.GetConcurentValue(_multiThreadHelper,
                rm => rm.StoredRoom.Users.FirstOrDefault(x => x.UserIdentifier == userId)?.IsAdmin ?? false);
            return res.sc && res.res;
        }


        private List<PlanitUser> ClearHideData(RoomSatus roomStatus, string currentUserId, List<PlanitUser> users)
        {
            if (roomStatus != RoomSatus.AllCanVote)
            {
                return users;
            }
            var user = users.FirstOrDefault(x => x.UserIdentifier == currentUserId);
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

    }
}
