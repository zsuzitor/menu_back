
using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System.Collections.Generic;

namespace BO.Models.CodeReviewApp.DAL.Domain
{
    public class ProjectUser : IDomainRecord<long>
    {
        public long Id { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public bool Deactivated { get; set; }
        public string NotifyEmail { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public long? MainAppUserId { get; set; }
        public User MainAppUser { get; set; }

        public List<CommentReview> Comments { get; set; }
        public List<TaskReview> CreateByUser { get; set; }
        public List<TaskReview> ReviewByUser { get; set; }

        public ProjectUser()
        {
            Comments = new List<CommentReview>();
            CreateByUser = new List<TaskReview>();
            ReviewByUser = new List<TaskReview>();

        }
    }
}
