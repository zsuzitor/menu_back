using PlanitPoker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Repositories.Interfaces
{
    public interface IPlanitPokerRepository
    {

        Task<Room> CreateRoomWithUser(string roomName, string password, PlanitUser user);
        Task<bool> AddTimeAliveRoom(string roomName);
        Task<bool> AddTimeAliveRoom(Room room);

        Task<bool> AddUserIntoRoom(string roomName, PlanitUser user);
        Task<bool> AddUserIntoRoom(Room room, PlanitUser user);
        Task<List<PlanitUser>> GetAllUsers(Room room);
        /// <summary>
        /// просто получить пользователей
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        Task<List<PlanitUser>> GetAllUsers(string roomName);
        Task<bool> ChangeUserName(string roomName, string userId, string newUserName);
        




        Task<bool> ClearVotes(Room room);
        Task<bool> ChangeVote(Room room,string userId, int vote);


        Task<bool> KickFromRoom(string roomName, string userIdRequest, string userId);
        Task<bool> KickFromRoom(Room room, string userIdRequest, string userId);
        Task<bool> LeaveFromRoom(string roomName, string userId);
        Task<bool> LeaveFromRoom(Room room, string userId);


        Task<bool> ChangeStatusIfCan(string roomName, string userId, RoomSatus newStatus);

        Task<bool> ChangeStatusIfCan(Room room,string userId, RoomSatus newStatus);

        Task<bool> RoomIsExist(string roomName);
        Task<Room> TryGetRoom(string roomName, string password);
        Task<Room> TryGetRoom(string roomName);
        Task<bool> UserIsAdmin(string roomName, string userId);
        Task<bool> UserIsAdmin(Room room, string userId);
        Task<bool> AddAdmin(string roomName, string userId);
        Task<bool> AddAdmin(Room room, string userId);
        Task<List<string>> GetAdminsId(Room room);
        Task<List<string>> GetAdminsId(string roomName);
        Task ClearOldRooms();

        Task<bool> AddNewStatusToUser(string roomName, string userId, string newRole,string userIdRequest);
        Task<bool> RemoveStatusUser(string roomName, string userId, string oldRole, string userIdRequest);

        Task<bool> AddNewStory(string roomName, string userId, Story newStory);
        Task<bool> ChangeStory(string roomName, string userId, Story newData);
        Task<bool> ChangeCurrentStory(string roomName, string userId, long storyId);
        Task<bool> DeleteStory(string roomName, string userId, long storyId);

        Task<bool> MakeStoryComplete(string roomName, long storyId, string userId);
        Task<bool> MakeStoryComplete(Room room, long storyId, string userId);


    }
}
