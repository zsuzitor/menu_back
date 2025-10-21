using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.DTO
{
    public class GetTasksByFilter
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public long? CreatorId { get; set; }
        public long? ExecutorId { get; set; }
        public long? StatusId { get; set; }
        public long? SprintId { get; set; }
        public long? LabelId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


    }
}
