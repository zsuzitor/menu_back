using Common.Models;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Services
{
    public class PlanitPokerService : IPlanitPokerService
    {
        IPlanitPokerRepository _planitRepo;
        MultiThreadHelper _multiThreadHelper;
        public PlanitPokerService(IPlanitPokerRepository planitRepo, MultiThreadHelper multiThreadHelper)
        {
            _multiThreadHelper = multiThreadHelper;
            _planitRepo = planitRepo;
        }

        public async Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userId)
        {
            var users = await _planitRepo.GetAllUsers(room);
            if (users==null||users.Count == 0)
            {
                return new List<PlanitUser>();
            }

            var roomStatusR = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Status);
            if (!roomStatusR.sc)
            {
                return new List<PlanitUser>();
            }

            return ClearHideData(roomStatusR.res, userId, users);
        }

        public async Task<List<PlanitUser>> GetAllUsersWithRight(string roomName, string userId)
        {
            var room = await _planitRepo.TryGetRoom(roomName);
            return await GetAllUsersWithRight(room, userId);
        }

        public async Task<StoredRoom> GetRoomInfoWithRight(string roomname, string currentUserId)
        {
            var room = await _planitRepo.TryGetRoom(roomname);
            return await GetRoomInfoWithRight(room, currentUserId);
        }

        public async Task<StoredRoom> GetRoomInfoWithRight(Room room, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return null;
            }

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


        //users - должна быть копия! тут без локов
        private List<PlanitUser> ClearHideData(RoomSatus roomStatus, string currentUserId, List<PlanitUser> users)
        {
            if (users == null)
            {
                return null;
            }

            users.ForEach(x => {
                if (x.Vote != null)
                {
                    x.HasVote = true;//todo хорошо бы вытащить из модели
                }
            });
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
