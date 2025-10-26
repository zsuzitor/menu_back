using BO.Models.TaskManagementApp.DAL.Domain;

namespace TaskManagementApp.Models.Returns
{
    public class WorkTaskRelationReturn
    {
        public long Id { get; set; }
        public long SubWorkTaskId { get; set; }
        public long MainWorkTaskId { get; set; }
        public long RelationType { get; set; }

        public WorkTaskRelationReturn(TaskRelation taskRelation)
        {
            Id = taskRelation.Id;
            SubWorkTaskId = taskRelation.SubWorkTaskId;
            MainWorkTaskId = taskRelation.MainWorkTaskId;
            RelationType = (int)taskRelation.RelationType;
        }

    }
}
