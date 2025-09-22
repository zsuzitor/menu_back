namespace Menu.Models.TaskManagementApp.Requests
{
    public class AddNewUserRequest
    {
        public string UserName { get; set; }
        public string MainAppUserEmail { get; set; }
        public long ProjectId { get; set; }
    }
}
