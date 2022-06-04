
using BO.Models.CodeReviewApp.DAL.Domain;

namespace CodeReviewApp.Models.Returns
{
    public sealed class ProjectInList
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ProjectInList(Project project)
        {
            if (project == null)
            {
                return;
            }

            Id = project.Id;
            Name = project.Name;
        }
    }
}
