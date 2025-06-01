using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class TaskReviewStatusReturn
    {

        public long Id { get; set; }
        public string Name { get; set; }

        public TaskReviewStatusReturn(TaskReviewStatus model)
        {
            Id = model.Id;
            Name = model.Name;
        }
    }
}
