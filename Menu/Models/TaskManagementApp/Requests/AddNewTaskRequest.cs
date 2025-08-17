using Microsoft.AspNetCore.Mvc;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class AddNewTaskRequest
    {
        public string TaskName { get; set; }
        public long? TaskReviwerId { get; set; }
        public string TaskLink { get; set; }
        public long ProjectId { get; set; }
        public long StatusId { get; set; }
        public string Description { get; set; }
    }
}
