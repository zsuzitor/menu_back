using BO.Models.TaskManagementApp.DAL.Domain;
using Menu.Models.TaskManagementApp.Requests;
using Nest;
using static System.Net.Mime.MediaTypeNames;

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
    }
}
