using System.Collections.Generic;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class UpdateTaskLabels
    {
        public List<long> LabelId { get; set; }
        public long TaskId { get; set; }

    }
}
