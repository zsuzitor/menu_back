using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Services
{
    public interface IPlanitPokerService
    {
        Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userId);
        Task<List<PlanitUser>> GetAllUsersWithRight(string roomName, string userId);
        Task<StoredRoom> GetRoomInfoWithRight(string roomname, string currentUserId);
        Task<StoredRoom> GetRoomInfoWithRight(Room room, string currentUserId);
    }
}
