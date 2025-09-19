using BO.Models.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class WorkTaskLabelTask : IDomainRecord<long>
    {
        public long Id { get; set; }


        public long TaskId { get; set; }
        public WorkTask Task { get; set; }

        public long LabelId { get; set; }
        public WorkTaskLabel Label { get; set; }
    }
}
