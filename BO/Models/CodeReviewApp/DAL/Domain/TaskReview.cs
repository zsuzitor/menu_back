
namespace BO.Models.CodeReviewApp.DAL.Domain
{
    public enum CodeReviewTaskStatus
    {
        NeedReview,
        NeedFix,
        Done
    }

    public class TaskReview
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public long CreatorId { get; set; }
        public long? ReviewerId { get; set; }
        public CodeReviewTaskStatus Status { get; set; }

        public TaskReview()
        {
            Status = CodeReviewTaskStatus.NeedReview;
        }

    }
}
