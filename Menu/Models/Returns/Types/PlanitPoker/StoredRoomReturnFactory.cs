using WEB.Common.Models.Returns.Interfaces;
using PlanitPoker.Models;
using PlanitPoker.Models.Returns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace Menu.Models.Returns.Types.PlanitPoker
{
    public class StoredRoomReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is StoredRoom objTyped)
            {
                return new StoredRoomReturn(objTyped);
            }

            if (obj is IEnumerable<StoredRoom> objTypedList)
            {
                return objTypedList?.Select(x => new StoredRoomReturn(x)).ToList();
            }


            return obj;
        }
    }
}
