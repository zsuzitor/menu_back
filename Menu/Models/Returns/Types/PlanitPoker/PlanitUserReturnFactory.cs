using Menu.Models.Returns.Interfaces;
using PlanitPoker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types.PlanitPoker
{
    public class PlanitUserReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is PlanitUser objTyped)
            {
                return new PlanitUserReturn(objTyped);
            }

            if (obj is IEnumerable<PlanitUser> objTypedList)
            {
                return objTypedList?.Select(x => new PlanitUserReturn(x)).ToList();
            }


            return obj;
        }
    }


    public class PlanitUserReturn
    {
        [JsonPropertyName("id")]
        public string UserIdentifier { get; set; }//signalRUserId
        [JsonPropertyName("is_admin")]

        public bool IsAdmin { get; set; }//enum?
        [JsonPropertyName("name")]

        public string Name { get; set; }
        [JsonPropertyName("vote")]

        public int? Vote { get; set; }



        public PlanitUserReturn(PlanitUser obj)
        {
            UserIdentifier = obj.UserIdentifier;
            IsAdmin = obj.Role.Contains("Admin");
            Name = obj.Name;
            Vote = obj.Vote;
        }
    }
}
