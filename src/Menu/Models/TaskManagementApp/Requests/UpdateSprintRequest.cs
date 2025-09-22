using System;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class UpdateSprintRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
