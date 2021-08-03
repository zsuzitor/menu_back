using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Returns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Services
{
    public interface IPlanitPokerService
    {
        Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userId);
        Task<List<PlanitUser>> GetAllUsersWithRight(string roomName, string userId);

        //todo наверное стоит создать аналогичную сущность без return и тут заюзать
        Task<RoomInfoReturn> GetRoomInfoWithRight(string roomName, string currentUserId);

        Task<RoomInfoReturn> GetRoomInfoWithRight(Room room, string currentUserId);
        Task<EndVoteInfo> GetEndVoteInfo(string roomName);

        Task<EndVoteInfo> GetEndVoteInfo(Room room);

        //Task<List<string>> DeleteRoom(string roomName);
        Task<Room> DeleteRoom(string roomName, string userConnectionIdRequest);

        Task<bool> SaveRoom(string roomName, string userConnectionIdRequest);



        Task<Room> CreateRoomWithUser(string roomName, string password, PlanitUser user);
        Task<bool> AddTimeAliveRoom(string roomName);
        Task<bool> AddTimeAliveRoom(Room room);

        Task<(bool sc, string oldConnectionId)> AddUserIntoRoom(string roomName, PlanitUser user);
        Task<(bool sc, string oldConnectionId)> AddUserIntoRoom(Room room, PlanitUser user);
        Task<List<PlanitUser>> GetAllUsers(Room room);

        /// <summary>
        /// просто получить пользователей
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        Task<List<PlanitUser>> GetAllUsers(string roomName);

        Task<(bool sc, string userId)> ChangeUserName(string roomName, string connectionUserId, string newUserName);

        Task<bool> ClearVotes(Room room);
        Task<(bool sc, string userId)> ChangeVote(Room room, string connectionUserId, string vote);


        Task<(PlanitUser user, bool sc)> KickFromRoom(string roomName, string userConnectionIdRequest, string userId);
        Task<(PlanitUser user, bool sc)> KickFromRoom(Room room, string userConnectionIdRequest, string userId);
        Task<(bool sc, string userId)> LeaveFromRoom(string roomName, string userConnectionIdRequest);
        Task<(bool sc, string userId)> LeaveFromRoom(Room room, string userConnectionIdRequest);


        Task<bool> ChangeStatusIfCan(string roomName, string userConnectionIdRequest, RoomSatus newStatus);

        Task<bool> ChangeStatusIfCan(Room room, string userConnectionIdRequest, RoomSatus newStatus);

        //Task<bool> RoomIsExist(string roomName);
        Task<Room> TryGetRoom(string roomName, string password);
        Task<Room> TryGetRoom(string roomName, bool cacheOnly = true);
        Task<bool> UserIsAdmin(string roomName, string userConnectionIdRequest);

        Task<bool> UserIsAdmin(Room room, string userConnectionIdRequest);

        //Task<bool> AddAdmin(string roomName, string userId);
        //Task<bool> AddAdmin(Room room, string userId);
        Task<List<string>> GetAdminsId(Room room);
        Task<List<string>> GetAdminsId(string roomName);
        Task ClearOldRooms();

        Task<bool> AddNewRoleToUser(string roomName, string userId, string newRole, string userConnectionIdRequest);
        Task<bool> RemoveRoleUser(string roomName, string userId, string oldRole, string userConnectionIdRequest);

        Task StartVote(string roomName, string userConnectionIdRequest);
        Task<EndVoteInfo> EndVote(string roomName, string userConnectionIdRequest);

        Task<bool> AddNewStory(string roomName, string userConnectionIdRequest, Story newStory);
        Task<bool> ChangeStory(string roomName, string userConnectionIdRequest, Story newData);
        Task<bool> ChangeCurrentStory(string roomName, string userConnectionIdRequest, string storyId);
        Task<bool> DeleteStory(string roomName, string userConnectionIdRequest, string storyId);

        Task<List<Story>> LoadNotActualStories(string roomName);


        /// <summary>
        /// возвращает копию если все прошло ок
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="storyId"></param>
        /// <param name="userConnectionIdRequest"></param>
        /// <returns></returns>
        Task<(string oldId, Story story)> MakeStoryComplete(string roomName, string storyId,
            string userConnectionIdRequest);

        /// <summary>
        /// возвращает копию если все прошло ок
        /// </summary>
        /// <param name="room"></param>
        /// <param name="storyId"></param>
        /// <param name="userConnectionIdRequest"></param>
        /// <returns></returns>
        Task<(string oldId, Story story)> MakeStoryComplete(Room room, string storyId, string userConnectionIdRequest);


        //string NormalizeRoomName(string roomName);
    }
}
