
using BO.Models.DAL.Domain;

namespace BO.Models.CodeReviewApp.DAL.Domain
{
    public class ProjectUser
    {
        public long Id { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public bool Deactivated { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public long? MainAppUserId { get; set; }
        public User MainAppUser { get; set; }
    }
}
