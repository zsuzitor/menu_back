
//using PlanitPoker.Models.Returns;
//using PlanitPoker.Models.Entity;
//using System.Collections.Generic;
//using System.Linq;
//using Common.Models;

//namespace Menu.Models.Returns.Types.PlanitPoker
//{
//    public sealed class StoredRoomReturnFactory : IReturnObjectFactory
//    {
//        public object GetObjectReturn(object obj)
//        {
//            if (obj == null)
//            {
//                return null;
//            }

//            if (obj is StoredRoom objTyped)
//            {
//                return new StoredRoomReturn(objTyped);
//            }

//            if (obj is IEnumerable<StoredRoom> objTypedList)
//            {
//                return objTypedList?.Select(x => new StoredRoomReturn(x)).ToList();
//            }


//            return obj;
//        }
//    }
//}
