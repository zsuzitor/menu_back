using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class ProjectFullInfoReturn
    {
        public List<ProjectUserReturn> Users { get; set; }
        public List<WorkTaskStatusReturn> Statuses { get; set; }
        public List<ProjectSprintReturn> Sprints { get; set; }
        public List<ProjectLabelReturn> Labels { get; set; }
    }
}
