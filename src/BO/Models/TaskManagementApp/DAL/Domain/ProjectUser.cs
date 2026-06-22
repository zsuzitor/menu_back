
using BO.Models.DAL;
using BO.Models.DAL.Domain;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public sealed class ProjectUser : IDomainRecord<long>
    {
        public long Id { get; set; }
        //public bool IsAdmin { get; set; }
        public UserRoleEnum Role { get; set; }
        //public bool Deactivated { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public long? MainAppUserId { get; set; }
        public User MainAppUser { get; set; }



        

        public byte[] RowVersion { get; set; }

        public ProjectUser()
        {

        }
    }
}
