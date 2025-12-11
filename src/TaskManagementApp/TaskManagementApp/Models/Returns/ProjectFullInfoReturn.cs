using System.Collections.Generic;

namespace TaskManagementApp.Models.Returns
{
    public class ProjectFullInfoReturn
    {
        public List<ProjectUserReturn> Users { get; set; }
        public List<WorkTaskStatusReturn> Statuses { get; set; }
        public List<ProjectSprintReturn> Sprints { get; set; }
        public List<ProjectLabelReturn> Labels { get; set; }
        public List<PresetReturn> Presets { get; set; }
    }
}
