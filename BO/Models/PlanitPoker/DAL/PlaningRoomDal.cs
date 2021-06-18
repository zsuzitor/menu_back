

using System.Collections.Generic;

namespace BO.Models.PlaningPoker.DAL
{
    public class PlaningRoomDal
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }//TODO потом зашифровать

        public List<PlaningStoryDal> Stories { get; set; }
        public List<PlaningRoomUserDal> Users { get; set; }


    }
}
