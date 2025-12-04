using BO.Models.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class Preset : IDomainRecord<long>
    {

        public long Id { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }


        public long? CreatorId { get; set; }
        public ProjectUser Creator { get; set; }
        public long? ExecutorId { get; set; }
        public ProjectUser Executor { get; set; }

        public long? StatusId { get; set; }
        public WorkTaskStatus Status { get; set; }

        public long? SprintId { get; set; }

        public WorkTaskSprintRelation Sprint { get; set; }

        public List<WorkTaskLabelPresetRelation> Labels { get; set; }

        public byte[] RowVersion { get; set; }

        public Preset()
        {
            Labels = new List<WorkTaskLabelPresetRelation>();
        }
    }
}
