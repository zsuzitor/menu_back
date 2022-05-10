
using BO.Models.DAL;
using System;

namespace BO.Models.CodeReviewApp.DAL.Domain
{
    public class CommentReview : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }

        public long TaskId { get; set; }
        public TaskReview Task { get; set; }

        public long CreatorId { get; set; }
        public ProjectUser Creator { get; set; }

        public CommentReview()
        {
            CreateDate = DateTime.Now;
        }
    }
}
