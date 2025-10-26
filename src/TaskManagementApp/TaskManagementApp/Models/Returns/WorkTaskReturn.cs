using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementApp.Models.Returns
{
    public sealed class WorkTaskReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? CreatorId { get; set; }
        public long? ExecutorId { get; set; }
        public long? StatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public List<long> SprintId { get; set; }
        public List<long> LabelsId { get; set; }

        public List<WorkTaskCommentReturn> Comments { get; set; }

        public List<WorkTaskRelationReturn> Relations { get; set; }

        public WorkTaskReturn(WorkTask task)
        {
            var relation = (task.MainWorkTasksRelation ?? new List<TaskRelation>()).ToList();
            relation.AddRange((task.SubWorkTasksRelation ?? new List<TaskRelation>()).ToList());
            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            CreateDate = task.CreateDate;
            LastUpdateDate = task.LastUpdateDate;
            CreatorId = task.CreatorId;
            ExecutorId = task.ExecutorId;
            StatusId = task.StatusId;
            SprintId = (task.Sprints ?? new List<WorkTaskSprintRelation>()).Select(x => x.SprintId).ToList();
            LabelsId = (task.Labels ?? new List<WorkTaskLabelTaskRelation>()).Select(x => x.LabelId).ToList();
            Comments = (task.Comments ?? new List<WorkTaskComment>()).Select(x => new WorkTaskCommentReturn(x)).ToList();
            Relations = relation.Select(x => new WorkTaskRelationReturn(x)).ToList();
        }
    }
}
