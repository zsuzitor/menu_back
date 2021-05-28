using PlanitPoker.Models.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace PlanitPoker.Models.Repositories
{
    public class PlanitPokerRepository: IPlanitPokerRepository
    {
        private static ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();


    }
}
