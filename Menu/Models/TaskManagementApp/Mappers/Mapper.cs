using BO.Models.TaskManagementApp.DAL.Domain;
using Menu.Models.TaskManagementApp.Requests;
using Nest;

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
        
    }
}
