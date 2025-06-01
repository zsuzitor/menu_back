using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementApp.Models.Returns
{
    public sealed class TaskReviewReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? CreatorId { get; set; }
        public long? ReviewerId { get; set; }
        public long? StatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public List<CommentReviewReturn> Comments { get; set; }

        public TaskReviewReturn(TaskReview task)
        {
            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            CreateDate = task.CreateDate;
            LastUpdateDate = task.LastUpdateDate;
            CreatorId = task.CreatorId;
            ReviewerId = task.ReviewerId;
            StatusId = task.StatusId;
            Comments = (task.Comments ?? new List<CommentReview>()).Select(x => new CommentReviewReturn(x)).ToList();
        }
    }
}
