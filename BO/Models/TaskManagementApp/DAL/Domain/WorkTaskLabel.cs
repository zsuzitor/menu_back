using BO.Models.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class WorkTaskLabel : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }


        public List<WorkTaskLabelTask> Tasks { get; set; }
        public byte[] RowVersion { get; set; }

        public WorkTaskLabel()
        {
            Tasks = new List<WorkTaskLabelTask>();
        }
    }
}
