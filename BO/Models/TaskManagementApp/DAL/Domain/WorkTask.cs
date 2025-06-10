
using BO.Models.DAL;
using System;
using System.Collections.Generic;

namespace BO.Models.TaskManagementApp.DAL.Domain
{

    public sealed class WorkTask : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //public string Link { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Description { get; set; }


        /// <summary>
        /// id main app пользака который именно завел задачу в приложение
        /// </summary>
        public long CreatorEntityId { get; set; }
        public long? StatusId { get; set; }
        public WorkTaskStatus Status { get; set; }


        public long? SprintId { get; set; }
        public WorkTaskSprint Sprint { get; set; }



        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public long CreatorId { get; set; }
        public ProjectUser Creator { get; set; }
        public long? ExecutorId { get; set; }

        public ProjectUser Executor { get; set; }

        public List<WorkTaskComment> Comments { get; set; }


        public List<WorkTimeLog> WorkTimeLogs { get; set; }


        public byte[] RowVersion { get; set; }

        public WorkTask()
        {
            Comments = new List<WorkTaskComment>();
            WorkTimeLogs = new List<WorkTimeLog>();
        }

        public WorkTask CopyPlaneProp()
        {
            var res = new WorkTask();
            res.Id = this.Id;
            res.Name = this.Name;
            res.CreateDate = this.CreateDate;
            res.LastUpdateDate = this.LastUpdateDate;
            res.Description = this.Description;
            res.CreatorEntityId = this.CreatorEntityId;
            res.StatusId = this.StatusId;
            res.ProjectId = this.ProjectId;
            res.CreatorId = this.CreatorId;
            res.ExecutorId = this.ExecutorId;


            return res;
        }

    }
}
