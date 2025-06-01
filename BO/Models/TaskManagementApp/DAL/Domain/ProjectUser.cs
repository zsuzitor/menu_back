
using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public sealed class ProjectUser : IDomainRecord<long>
    {
        public long Id { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public bool Deactivated { get; set; }
        public string NotifyEmail { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public long? MainAppUserId { get; set; }
        public User MainAppUser { get; set; }

        public List<WorkTaskComment> Comments { get; set; }
        public List<WorkTask> CreateByUser { get; set; }
        public List<WorkTask> ExecuteByUser { get; set; }


        public byte[] RowVersion { get; set; }

        public ProjectUser()
        {
            Comments = new List<WorkTaskComment>();
            CreateByUser = new List<WorkTask>();
            ExecuteByUser = new List<WorkTask>();

        }
    }
}
