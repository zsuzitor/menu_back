

using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

namespace BO.Models.PlaningPoker.DAL
{
    public sealed class PlaningRoomDal: IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public long? ImageId { get; set; }
        public CustomImage Image { get; set; }


        public List<PlaningStoryDal> Stories { get; set; }
        public List<PlaningRoomUserDal> Users { get; set; }
        public string Cards { get; set; }

        public byte[] RowVersion { get; set; }


        public PlaningRoomDal()
        {
            Stories = new List<PlaningStoryDal>();
            Users = new List<PlaningRoomUserDal>();
        }

    }
}
