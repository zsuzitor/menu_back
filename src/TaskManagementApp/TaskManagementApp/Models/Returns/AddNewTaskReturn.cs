using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class AddNewTaskReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //public string Link { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string Description { get; set; }

        public WorkTaskStatusReturn Status { get; set; }




        public long CreatorId { get; set; }
        public long? ExecutorId { get; set; }


        public AddNewTaskReturn(WorkTask task)
        {

            Id = task.Id;
            Name = task.Name;
            CreatorId = task.CreatorId;
            ExecutorId = task.ExecutorId;
            Status = new WorkTaskStatusReturn(task.Status);
            CreateDate = task.CreateDate.ToString();
            LastUpdateDate = task.LastUpdateDate.ToString();
            Description = task.Description;
        }
    }
}
