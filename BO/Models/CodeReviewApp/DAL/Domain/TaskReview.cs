
using BO.Models.DAL;
using System.Collections.Generic;

namespace BO.Models.CodeReviewApp.DAL.Domain
{
    //public enum CodeReviewTaskStatus
    //{
    //    NeedReview,
    //    NeedFix,
    //    Done,
    //    InProgress,
    //}

    public sealed class TaskReview : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }


        /// <summary>
        /// id main app пользака который именно завел задачу в приложение
        /// </summary>
        public long CreatorEntityId { get; set; }
        public long? StatusId { get; set; }
        public TaskReviewStatus Status { get; set; }



        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public long CreatorId { get; set; }
        public ProjectUser Creator { get; set; }
        public long? ReviewerId { get; set; }
        public ProjectUser Reviewer { get; set; }

        public List<CommentReview> Comments { get; set; }



        public byte[] RowVersion { get; set; }

        public TaskReview()
        {
            //Status = CodeReviewTaskStatus.NeedReview;
            Comments = new List<CommentReview>();
        }

    }
}
