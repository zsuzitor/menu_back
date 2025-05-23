using BO.Models.CodeReviewApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeReviewApp.Models.Returns
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
