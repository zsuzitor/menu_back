using System;
using PlaningPoker.Models.Enums;
using PlaningPoker.Models.Returns;
using PlaningPoker.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PlaningPoker.Models.Services
{
    public interface IPlaningPokerService
    {
        Task<List<PlaningUser>> GetAllUsersWithRight(Room room, string userId);
        Task<List<PlaningUser>> GetAllUsersWithRightAsync(string roomName, string userId);

        //todo наверное стоит создать аналогичную сущность без return и тут заюзать
        Task<RoomInfoReturn> GetRoomInfoWithRightAsync(string roomName, string currentUserId);

        Task<RoomInfoReturn> GetRoomInfoWithRight(Room room, string currentUserId);
        Task<EndVoteInfo> GetEndVoteInfoAsync(string roomName);

        Task<EndVoteInfo> GetEndVoteInfo(Room room);
        Task<EndVoteInfo> RecalcEndVoteInfo(Room room);

        //Task<List<string>> DeleteRoom(string roomName);
        Task<Room> DeleteRoomAsync(string roomName, string userConnectionIdRequest);

        Task<RoomWasSavedUpdate>
            SaveRoomAsync(string roomName, string userConnectionIdRequest);



        Task<Room> CreateRoomWithUserAsync(string roomName, string password, PlaningUser user);
        Task<DateTime> AddTimeAliveRoomAsync(string roomName);
        Task<DateTime> AddTimeAliveRoom(Room room);
        Task<bool> SetRoomCards(Room room, string userConnectionId, List<string> cards);
        Task<List<RoomShortInfo>> GetRoomsAsync(long userId);
        Task<string> ChangeRoomImageAsync(string roomName, long userId, IFormFile image);

        Task<(bool sc, string oldConnectionId)> AddUserIntoRoomAsync(string roomName, PlaningUser user);
        Task<(bool sc, string oldConnectionId)> AddUserIntoRoomAsync(Room room, PlaningUser user);
        Task<List<PlaningUser>> GetAllUsers(Room room);

        /// <summary>
        /// просто получить пользователей
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        Task<List<PlaningUser>> GetAllUsersAsync(string roomName);

        Task<(bool sc, string userId)> ChangeUserNameAsync(string roomName, string connectionUserId, string newUserName);

        Task<bool> ClearVotes(Room room);
        Task<(bool sc, string userId)> ChangeVote(Room room, string connectionUserId, string vote);
        Task<bool> AllVotedAsync(Room room);


        Task<(PlaningUser user, bool sc)> KickFromRoomAsync(string roomName, string userConnectionIdRequest, string userId);
        Task<(PlaningUser user, bool sc)> KickFromRoomAsync(Room room, string userConnectionIdRequest, string userId);
        Task<(bool sc, string userId)> LeaveFromRoomAsync(string roomName, string userConnectionIdRequest);
        Task<(bool sc, string userId)> LeaveFromRoom(Room room, string userConnectionIdRequest);


        Task<bool> ChangeStatusIfCanAsync(string roomName, string userConnectionIdRequest, RoomSatus newStatus);

        Task<bool> ChangeStatusIfCanAsync(Room room, string userConnectionIdRequest, RoomSatus newStatus);

        //Task<bool> RoomIsExist(string roomName);

        /// <summary>
        /// тянет еще и из бд
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Room> TryGetRoomAsync(string roomName, string password);
        Task<Room> TryGetRoomAsync(string roomName, bool cacheOnly = true);
        Task<bool> UserIsAdminAsync(string roomName, string userConnectionIdRequest);

        Task<bool> UserIsAdmin(Room room, string userConnectionIdRequest);

        //Task<bool> AddAdmin(string roomName, string userId);
        //Task<bool> AddAdmin(Room room, string userId);
        Task<List<string>> GetAdminsIdAsync(Room room);
        Task<List<string>> GetAdminsIdAsync(string roomName);

        Task<bool> AddNewRoleToUserAsync(string roomName, string userId, string newRole, string userConnectionIdRequest);
        Task<bool> RemoveRoleUserAsync(string roomName, string userId, string oldRole, string userConnectionIdRequest);

        Task StartVoteAsync(string roomName, string userConnectionIdRequest);
        Task<EndVoteInfo> EndVoteAsync(string roomName, string userConnectionIdRequest);

        Task<bool> AddNewStoryAsync(string roomName, string userConnectionIdRequest, Story newStory);
        Task<bool> ChangeStoryAsync(string roomName, string userConnectionIdRequest, Story newData);
        Task<bool> ChangeCurrentStoryAsync(string roomName, string userConnectionIdRequest, string storyId);
        Task<bool> DeleteStoryAsync(string roomName, string userConnectionIdRequest, string storyId);

        [Obsolete]
        Task<List<Story>> LoadNotActualStoriesAsync(string roomName);
        Task<List<Story>> GetNotActualStoriesAsync(string roomName, string userConnectionId, int pageNumber, int pageSize);


        /// <summary>
        /// возвращает копию если все прошло ок
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="storyId"></param>
        /// <param name="userConnectionIdRequest"></param>
        /// <returns></returns>
        Task<(string oldId, Story story)> MakeStoryCompleteAsync(string roomName, string storyId,
            string userConnectionIdRequest);

        /// <summary>
        /// возвращает копию если все прошло ок
        /// </summary>
        /// <param name="room"></param>
        /// <param name="storyId"></param>
        /// <param name="userConnectionIdRequest"></param>
        /// <returns></returns>
        Task<(string oldId, Story story)> MakeStoryCompleteAsync(Room room, string storyId, string userConnectionIdRequest);


        Task HandleInRoomsMemoryAsync(bool clearRooms = true, bool force = false);
        Task HandleInRoomsMemoryAsync();
        Task<bool> ChangeRoomPasswordAsync(string roomName, string userConnectionId, string oldPassword, string newPassword);
    }
}