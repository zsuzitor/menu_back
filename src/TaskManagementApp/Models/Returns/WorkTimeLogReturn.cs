
using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class WorkTimeLogReturn
    {


        public long Id { get; set; }
        public string Comment { get; set; }
        public long TimeMinutes { get; set; }
        /// <summary>
        /// день за которое списание
        /// </summary>
        public DateTime DayOfLog { get; set; }
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// начало работы для списаний диапазоном
        /// 
        /// </summary>
        public DateTime? RangeStartOfLog { get; set; }
        /// <summary>
        /// конец работы для списаний диапазоном
        /// </summary>
        public DateTime? RangeEndOfLog { get; set; }


        public long WorkTaskId { get; set; }


        public long ProjectUserId { get; set; }


        public WorkTimeLogReturn(WorkTimeLog obj)
        {
            Id = obj.Id;
            Comment = obj.Comment;
            TimeMinutes = obj.TimeMinutes;
            DayOfLog = obj.DayOfLog;
            CreationTime = obj.CreationTime;
            WorkTaskId = obj.WorkTaskId;
            ProjectUserId = obj.ProjectUserId;
            RangeEndOfLog = obj.RangeEndOfLog;
            RangeStartOfLog = obj.RangeStartOfLog;

        }
    }
}
