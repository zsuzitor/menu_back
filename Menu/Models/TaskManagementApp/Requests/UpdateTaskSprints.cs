using System.Collections.Generic;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class UpdateTaskSprints
    {
        public List<long> SprintId { get; set; }
        public long TaskId { get; set; }
    }
}
