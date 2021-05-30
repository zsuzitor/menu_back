using Common.Models;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using System.Collections.Concurrent;
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
            if (room == null || string.IsNullOrWhiteSpace(user?.UserIdentifier))
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
            });

            return true;
        }

        public async Task<bool> ChangeStatus(string roomName, RoomSatus newStatus)
        {
            var room = await TryGetRoom(roomName);
            return await ChangeStatus(room, newStatus);
        }

        public async Task<bool> ChangeStatus(Room room, RoomSatus newStatus)
        {
            if (room == null)
            {
                return false;
            }

            room.SetConcurentValue<Room>(_multiThreadHelper, rm =>
            {
                rm.StoredRoom.Status = newStatus;
            });

            return true;
        }

        public Task<bool> ChangeVote(Room room, string userId, int vote)
        {
            throw new System.NotImplementedException();
        }

        public Task ClearOldRooms()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ClearVotes(Room room)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Room> CreateRoomWithUser(string roomname, string password, PlanitUser user)
        {
            var roomData = new StoredRoom(roomname, password);
            var room = new Room(roomData);
            var added = Rooms.TryAdd(roomname, room);
            if (added)
            {
                return room;
            }

            return null;
        }

        public Task<bool> KickFromRoom(string roomName, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> KickFromRoom(Room room, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RoomIsExist(string roomName)
        {
            throw new System.NotImplementedException();
        }

        public Task<Room> TryGetRoom(string roomName, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<Room> TryGetRoom(string roomName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UserIsAdmin(string roomName, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UserIsAdmin(Room room, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
