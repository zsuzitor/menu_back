

using BO.Models.TaskManagementApp.DAL.Domain;

namespace TaskManagementApp.Models.Returns
{
    public class ProjectLabelReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }

        public ProjectLabelReturn(WorkTaskLabel data)
        {
            Id = data.Id;
            Name = data.Name;
            ProjectId = data.ProjectId;
        }
    }
}
