using BO.Models.CodeReviewApp.DAL.Domain;

namespace CodeReviewApp.Models.Returns
{
    public class ProjectUserReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ProjectUserReturn(ProjectUser user)
        {
            Id = user.Id;
            Name = user.UserName;
        }
    }
}
