using System;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class AddNewSprintRequest
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
