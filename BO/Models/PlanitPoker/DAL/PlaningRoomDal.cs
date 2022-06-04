

using BO.Models.DAL;
using System.Collections.Generic;

namespace BO.Models.PlaningPoker.DAL
{
    public sealed class PlaningRoomDal: IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }//может тоже хешить?
        public string Password { get; set; }//TODO потом зашифровать

        public List<PlaningStoryDal> Stories { get; set; }
        public List<PlaningRoomUserDal> Users { get; set; }

        public PlaningRoomDal()
        {
            Stories = new List<PlaningStoryDal>();
            Users = new List<PlaningRoomUserDal>();
        }

    }
}
