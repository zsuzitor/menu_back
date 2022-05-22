using BO.Models.CodeReviewApp.DAL.Domain;

namespace CodeReviewApp.Models.Returns
{
    public class ProjectUserReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public ProjectUserReturn(ProjectUser user)
        {
            Id = user.Id;
            Name = user.UserName;
            Email = user.NotifyEmail;
            IsAdmin = user.IsAdmin;
        }
    }
}
