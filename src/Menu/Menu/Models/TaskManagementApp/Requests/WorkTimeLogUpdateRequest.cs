using System;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class WorkTimeLogUpdateRequest
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string Text { get; set; }
        public long Minutes { get; set; }
        public DateTime DayOfLog { get; set; }
        public DateTime? RangeStartOfLog { get; set; }
        public DateTime? RangeEndOfLog { get; set; }
    }
}
