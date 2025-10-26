using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class GetProjectTasksReturn
    {
        public List<WorkTaskReturn> Tasks{ get; set; }
        public long TasksCount { get; set; }
    }
}
