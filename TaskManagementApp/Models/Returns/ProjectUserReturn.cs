using BO.Models.TaskManagementApp.DAL.Domain;

namespace TaskManagementApp.Models.Returns
{
    public sealed class ProjectUserReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public long? MainAppUserId { get; set; }
        public bool Deactivated { get; set; }

        public ProjectUserReturn(ProjectUser user)
        {
            Id = user.Id;
            Name = user.UserName;
            Email = user.NotifyEmail;
            IsAdmin = user.IsAdmin;
            MainAppUserId = user.MainAppUserId;
            Deactivated = user.Deactivated;
        }
    }
}
