
using BO.Models.DAL;
using BO.Models.DAL.Domain;
using System;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public sealed class WorkTaskComment : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }

        public long TaskId { get; set; }
        public WorkTask Task { get; set; }

        public long CreatorId { get; set; }
        public User Creator { get; set; }


        public byte[] RowVersion { get; set; }

        public WorkTaskComment()
        {
        }
    }
}
