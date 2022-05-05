using BO.Models.CodeReviewApp.DAL.Domain;

namespace CodeReviewApp.Models.Returns
{
    public class TaskReviewReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? CreatorId { get; set; }
        public long? ReviewerId { get; set; }
        public int Status { get; set; }

        public TaskReviewReturn(TaskReview task)
        {
            Id = task.Id;
            Name = task.Name;
            CreatorId = task.CreatorId;
            ReviewerId = task.ReviewerId;
            Status = (int)task.Status;
        }
    }
}
