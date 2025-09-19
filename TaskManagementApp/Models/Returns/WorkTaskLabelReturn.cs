using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class WorkTaskLabelReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public WorkTaskLabelReturn(WorkTaskLabel obj)
        {
            Id = obj.Id;
            Name = obj.Name;
        }
    }
}
