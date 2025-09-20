using BO.Models.DAL;
using System;
using System.Collections.Generic;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class ProjectSprint : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public List<WorkTaskSprintRelation> Tasks { get; set; }

        public byte[] RowVersion { get; set; }

        public ProjectSprint()
        {
            Tasks = new List<WorkTaskSprintRelation>();
        }
    }
}
