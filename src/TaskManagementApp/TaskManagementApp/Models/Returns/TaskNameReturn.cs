using System;
using System.Collections.Generic;
using System.Text;
using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Models.Returns
{
    public class TaskNameReturn
    {
        public long? Id { get; set; }
        public string Name { get; set; }

        public TaskNameReturn(TaskName t)
        {
            Id = t?.Id;
            Name = t?.Name;
        }
    }
}
