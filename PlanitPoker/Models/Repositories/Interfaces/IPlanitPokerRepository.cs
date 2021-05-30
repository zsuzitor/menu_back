using PlanitPoker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IPlanitPokerRepository
    {

        Task<Room> CreateRoomWithUser(string roomname, string password, PlanitUser user);
        Task<bool> AddTimeAliveRoom(string roomname);
        Task<bool> AddTimeAliveRoom(Room room);

        Task<bool> AddUserIntoRoom(string roomName, PlanitUser user);
        Task<bool> AddUserIntoRoom(Room room, PlanitUser user);
        Task<List<PlanitUser>> GetAllUsers(Room room);
        Task<List<PlanitUser>> GetAllUsers(string roomName);


        Task<bool> ClearVotes(Room room);
        Task<bool> ChangeVote(Room room,string userId, int vote);


        Task<bool> KickFromRoom(string roomName, string userId);
        Task<bool> KickFromRoom(Room room, string userId);
        Task<bool> ChangeStatus(string roomName, RoomSatus newStatus);

        Task<bool> ChangeStatus(Room room, RoomSatus newStatus);

        Task<bool> RoomIsExist(string roomName);
        Task<Room> TryGetRoom(string roomName, string password);
        Task<Room> TryGetRoom(string roomName);
        Task<bool> UserIsAdmin(string roomName, string userId);
        Task<bool> UserIsAdmin(Room room, string userId);
        Task<bool> AddAdmin(string roomName, string userId);
        Task<bool> AddAdmin(Room room, string userId);
        Task ClearOldRooms();
    }
}
