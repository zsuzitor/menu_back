using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class ProjectSprintReturn
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public ProjectSprintReturn(ProjectSprint obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            ProjectId = obj.ProjectId;
            StartDate = obj.StartDate;
            EndDate = obj.EndDate;
        }
    }
}
