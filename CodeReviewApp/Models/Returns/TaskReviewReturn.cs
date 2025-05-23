using BO.Models.CodeReviewApp.DAL.Domain;

namespace CodeReviewApp.Models.Returns
{
    public sealed class TaskReviewReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public long? CreatorId { get; set; }
        public long? ReviewerId { get; set; }
        public long? StatusId { get; set; }

        public TaskReviewReturn(TaskReview task)
        {
            Id = task.Id;
            Name = task.Name;
            Link = task.Link;
            CreatorId = task.CreatorId;
            ReviewerId = task.ReviewerId;
            StatusId = task.StatusId;
        }
    }
}
