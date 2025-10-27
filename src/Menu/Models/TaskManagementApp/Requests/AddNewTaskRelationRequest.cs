namespace Menu.Models.TaskManagementApp.Requests
{
    public class AddNewTaskRelationRequest
    {
        public long MainTaskid { get; set; }
        public long SubTaskid { get; set; }
        public long Type { get; set; }
    }
}
