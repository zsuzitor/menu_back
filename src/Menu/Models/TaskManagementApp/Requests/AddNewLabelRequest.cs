namespace Menu.Models.TaskManagementApp.Requests
{
    public class AddNewLabelRequest
    {
        public string Name { get; set; }
        public long ProjectId { get; set; }
    }
}
