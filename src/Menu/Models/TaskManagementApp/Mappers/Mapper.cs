using BO.Models.TaskManagementApp.DAL;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using Menu.Models.TaskManagementApp.Requests;
using Nest;
using System;
using TaskManagementApp.Models;

namespace Menu.Models.TaskManagementApp.Mappers
{
    public static class Mapper
    {
        public static WorkTimeLog Map(this WorkTimeLogCreateRequest obj)
        {
            return new WorkTimeLog()
            {

                Comment = obj.Text,
                DayOfLog = obj.DayOfLog,
                TimeMinutes = obj.Minutes,
                WorkTaskId = obj.TaskId,
                RangeEndOfLog = obj.RangeEndOfLog,
                RangeStartOfLog = obj.RangeStartOfLog,
            };
        }

        public static WorkTimeLog Map(this WorkTimeLogUpdateRequest obj)
        {
            return new WorkTimeLog()
            {
                Id = obj.Id,
                Comment = obj.Text,
                DayOfLog = obj.DayOfLog,
                TimeMinutes = obj.Minutes,
                WorkTaskId = obj.TaskId,
                RangeEndOfLog = obj.RangeEndOfLog,
                RangeStartOfLog = obj.RangeStartOfLog,
            };
        }

        public static WorkTask Map(this AddNewTaskRequest obj)
        {
            return new WorkTask()
            {
                Name = obj.TaskName,
                ExecutorId = obj.TaskReviwerId,
                ProjectId = obj.ProjectId,
                StatusId = obj.StatusId,
                Description = obj.Description,
            };
        }

        public static TaskRelation Map(this AddNewTaskRelationRequest obj)
        {
            if (!Enum.IsDefined(typeof(TaskRelationTypeEnum), obj.Type))
            {
                throw new SomeCustomException(Consts.ErrorConsts.BadRelationType);
            }

            return new TaskRelation()
            {
                MainWorkTaskId = obj.MainTaskid,
                RelationType = (TaskRelationTypeEnum)obj.Type,
                SubWorkTaskId = obj.SubTaskid,
            };
        }

        public static WorkTask Map(this UpdateTaskRequest obj)
        {
            return new WorkTask()
            {
                Id = obj.TaskId,
                Name = obj.Name,
                StatusId = obj.StatusId,
                //CreatorId = creatorId,
                ExecutorId = obj.ExecutorId,
                Description = obj.Description,
            };
        }

        public static ProjectSprint Map(this AddNewSprintRequest obj)
        {
            return new ProjectSprint()
            {
                Name = obj.Name,
                ProjectId = obj.ProjectId,
                StartDate = obj.StartDate,
                EndDate = obj.EndDate,

            };
        }

        public static ProjectSprint Map(this UpdateSprintRequest obj)
        {
            return new ProjectSprint()
            {
                Name = obj.Name,
                Id = obj.Id,
                StartDate = obj.StartDate,
                EndDate = obj.EndDate,

            };
        }

        

        public static WorkTaskLabel Map(this AddNewLabelRequest obj)
        {
            return new WorkTaskLabel()
            {
                Name = obj.Name,
                ProjectId = obj.ProjectId,
            };
        }

        public static WorkTaskLabel Map(this UpdateLabelRequest obj)
        {
            return new WorkTaskLabel()
            {
                Name = obj.Name,
                Id = obj.Id,
            };
        }

    }
}
