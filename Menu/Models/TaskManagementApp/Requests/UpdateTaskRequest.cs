
namespace Menu.Models.TaskManagementApp.Requests
{
    public class UpdateTaskRequest
    {
        public long TaskId { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public long? ExecutorId { get; set; }
        public string Description { get; set; }
    }
}
