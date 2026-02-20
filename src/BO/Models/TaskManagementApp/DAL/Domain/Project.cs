
using BO.Models.DAL;
using System.Collections.Generic;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public sealed class Project: IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public List<ProjectUser> Users { get; set; }
        //public List<string> LocalUsers { get; set; }
        public List<WorkTask> Tasks { get; set; }

        public List<WorkTaskStatus> TaskStatuses { get; set; }
        public List<ProjectSprint> Sprints { get; set; }

        public byte[] RowVersion { get; set; }

        public Project()
        {
            Users = new List<ProjectUser>();
            Tasks = new List<WorkTask>();
            TaskStatuses = new List<WorkTaskStatus>();
            Sprints = new List<ProjectSprint>();
        }

    }
}
