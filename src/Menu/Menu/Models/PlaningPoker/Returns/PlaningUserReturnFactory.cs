
using PlaningPoker.Models.Returns;
using PlaningPoker.Models.Entity;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Menu.Models.PlaningPoker.Returns
{
    public sealed class PlaningUserReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is PlaningUser objTyped)
            {
                return new PlaningUserReturn(objTyped);
            }

            if (obj is IEnumerable<PlaningUser> objTypedList)
            {
                return objTypedList?.Select(x => new PlaningUserReturn(x)).ToList();
            }


            return obj;
        }
    }



}
