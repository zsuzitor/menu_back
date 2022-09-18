using System;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Returns;
using PlanitPoker.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Services
{
    public interface IPlanitPokerService
    {
        List<PlanitUser> GetAllUsersWithRight(Room room, string userId);
        Task<List<PlanitUser>> GetAllUsersWithRightAsync(string roomName, string userId);

        //todo наверное стоит создать аналогичную сущность без return и тут заюзать
        Task<RoomInfoReturn> GetRoomInfoWithRightAsync(string roomName, string currentUserId);

        RoomInfoReturn GetRoomInfoWithRight(Room room, string currentUserId);
        Task<EndVoteInfo> GetEndVoteInfoAsync(string roomName);

        EndVoteInfo GetEndVoteInfo(Room room);

        //Task<List<string>> DeleteRoom(string roomName);
        Task<Room> DeleteRoomAsync(string roomName, string userConnectionIdRequest);

        Task<RoomWasSavedUpdate>
            SaveRoomAsync(string roomName, string userConnectionIdRequest);



        Task<Room> CreateRoomWithUserAsync(string roomName, string password, PlanitUser user);
        Task<DateTime> AddTimeAliveRoomAsync(string roomName);
        DateTime AddTimeAliveRoom(Room room);

        Task<(bool sc, string oldConnectionId)> AddUserIntoRoomAsync(string roomName, PlanitUser user);
        Task<(bool sc, string oldConnectionId)> AddUserIntoRoomAsync(Room room, PlanitUser user);
        List<PlanitUser> GetAllUsers(Room room);

        /// <summary>
        /// просто получить пользователей
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        Task<List<PlanitUser>> GetAllUsersAsync(string roomName);

        Task<(bool sc, string userId)> ChangeUserNameAsync(string roomName, string connectionUserId, string newUserName);

        bool ClearVotes(Room room);
        (bool sc, string userId) ChangeVote(Room room, string connectionUserId, string vote);
        Task<bool> AllVotedAsync(Room room);


        Task<(PlanitUser user, bool sc)> KickFromRoomAsync(string roomName, string userConnectionIdRequest, string userId);
        Task<(PlanitUser user, bool sc)> KickFromRoomAsync(Room room, string userConnectionIdRequest, string userId);
        Task<(bool sc, string userId)> LeaveFromRoomAsync(string roomName, string userConnectionIdRequest);
        (bool sc, string userId) LeaveFromRoom(Room room, string userConnectionIdRequest);


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

        bool UserIsAdmin(Room room, string userConnectionIdRequest);

        //Task<bool> AddAdmin(string roomName, string userId);
        //Task<bool> AddAdmin(Room room, string userId);
        Task<List<string>> GetAdminsIdAsync(Room room);
        Task<List<string>> GetAdminsIdAsync(string roomName);
        Task ClearOldRoomsAsync();

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
        Task<List<Story>> GetNotActualStoriesAsync(string roomName, int pageNumber, int pageSize);


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


        Task HandleInRoomsMemoryAsync();
    }
}
