using PlanitPoker.Models.Returns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Services
{
    public interface IPlanitPokerService
    {
        Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userId);
        Task<List<PlanitUser>> GetAllUsersWithRight(string roomName, string userId);
        Task<RoomInfoReturn> GetRoomInfoWithRight(string roomName, string currentUserId);//todo наверное стоит создать аналогичную сущность без return и тут заюзать
        Task<RoomInfoReturn> GetRoomInfoWithRight(Room room, string currentUserId);
        Task<EndVoteInfo> GetEndVoteInfo(string roomName);
        Task<EndVoteInfo> GetEndVoteInfo(Room room);

    }
}
