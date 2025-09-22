using BO.Models.TaskManagementApp.DAL.Domain;

namespace TaskManagementApp.Models.Returns
{
    public sealed class WorkTaskCommentReturn
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long TaskId { get; set; }
        public long CreatorId { get; set; }

        public WorkTaskCommentReturn(WorkTaskComment comment)
        {
            Id = comment.Id;
            Text = comment.Text;
            TaskId = comment.TaskId;
            CreatorId = comment.CreatorId;
        }
    }
}
