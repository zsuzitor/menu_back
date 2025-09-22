using BO.Models.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class WorkTaskStatus : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public List<WorkTask> Tasks { get; set; }
        public byte[] RowVersion { get; set; }

        public WorkTaskStatus()
        {
            Tasks = new List<WorkTask>();
        }

    }
}
