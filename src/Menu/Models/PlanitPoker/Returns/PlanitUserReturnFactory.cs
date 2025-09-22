
using PlanitPoker.Models.Returns;
using PlanitPoker.Models.Entity;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Menu.Models.PlanitPoker.Returns
{
    public sealed class PlanitUserReturnFactory : IReturnObjectFactory
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



}
