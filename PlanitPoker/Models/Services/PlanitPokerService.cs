using Common.Models;
using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Returns;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<PlanitUser>> GetAllUsersWithRight(Room room, string userConnectionId)
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

            return ClearHideData(roomStatusR.res, userConnectionId, users);
        }

        public async Task<List<PlanitUser>> GetAllUsersWithRight(string roomName, string userConnectionId)
        {
            var room = await _planitRepo.TryGetRoom(roomName);
            return await GetAllUsersWithRight(room, userConnectionId);
        }

        public async Task<RoomInfoReturn> GetRoomInfoWithRight(string roomName, string userConnectionId)
        {
            var room = await _planitRepo.TryGetRoom(roomName);
            return await GetRoomInfoWithRight(room, userConnectionId);
        }

        public async Task<RoomInfoReturn> GetRoomInfoWithRight(Room room, string userConnectionId)
        {
            if (string.IsNullOrWhiteSpace(userConnectionId))
            {
                return null;
            }

            if (room == null)
            {
                return null;
            }

            var roomInfo = await GetEndVoteInfo(room);//todo это можно сделать без локов тк ниже рума полностью копируется, можно создать новый метод
            var scRoom = room.GetConcurentValue(_multiThreadHelper, rm => rm.StoredRoom.Clone());
            if (!scRoom.sc)
            {
                return null;
            }

            //все склонированное, работаем обычно
            var resRoom = scRoom.res;
            resRoom.Password = null;
            ClearHideData(resRoom.Status, userConnectionId, resRoom.Users);

            var res = new RoomInfoReturn()
            {
                Room = new StoredRoomReturn(resRoom),
                EndVoteInfo = roomInfo,
            };
            return res;
        }


       public async Task<EndVoteInfo> GetEndVoteInfo(string roomName)
        {
            var room = await _planitRepo.TryGetRoom(roomName);
            return await GetEndVoteInfo(room);

        }

        public async Task<EndVoteInfo> GetEndVoteInfo(Room room)
        {
            (var res, bool sc) = room.GetConcurentValue(_multiThreadHelper, rm =>
            {
                if (rm.StoredRoom.Status != RoomSatus.CloseVote)
                {
                    return null;
                }

                return rm.StoredRoom.Users.Select(x => new { userId = x.PlaningAppUserId, vote = x.Vote ?? 0, hasVote = x.Vote.HasValue });
            });

            if (!sc)
            {
                //TODO
                return null;
            }

            var arrForMath = res.Where(x => x.hasVote);
            var result = new EndVoteInfo();
            if(arrForMath.Count() > 0)
            {
                result.MinVote = arrForMath.Min(x => x.vote);
                result.MaxVote = arrForMath.Max(x => x.vote);
                result.Average = arrForMath.Average(x => x.vote);
            }
            
            result.UsersInfo = res.Select(x => new EndVoteUserInfo() { Id = x.userId, Vote = x.vote }).ToList();
            return result;
        }


        


        //-----------------------------------------------------------------------------private

        //users - должна быть копия! тут без локов
        private List<PlanitUser> ClearHideData(RoomSatus roomStatus, string currentUserConnectionId, List<PlanitUser> users)
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
            var user = users.FirstOrDefault(x => x.UserConnectionId == currentUserConnectionId);
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
