using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.DTO
{
    public class GetTasksCountByFilter
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public long? CreatorId { get; set; }
        public long? ExecutorId { get; set; }
        public long? StatusId { get; set; }
        public long? SprintId { get; set; }
        public long? PresetId { get; set; }
        public List<long> LabelIds { get; set; }
    }
}
