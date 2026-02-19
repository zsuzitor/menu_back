namespace Menu.Models.TaskManagementApp.Requests
{
    public class GetProjectTasksByFilterRequest
    {
        public long ProjectId { get; set; }
        public string NameLike { get; set; }
        public long? CreatorId { get; set; }
        public long? ExecutorId { get; set; }
        public long? StatusId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long? SprintId { get; set; }
        public long? PresetId { get; set; }
        public long[] LabelId { get; set; }

    }
}
