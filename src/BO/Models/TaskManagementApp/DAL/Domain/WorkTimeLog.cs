using BO.Models.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class WorkTimeLog : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public long TimeMinutes { get; set; }

        /// <summary>
        /// начало работы для списаний диапазоном
        /// 
        /// </summary>
        public DateTime? RangeStartOfLog { get; set; }
        /// <summary>
        /// конец работы для списаний диапазоном
        /// </summary>
        public DateTime? RangeEndOfLog { get; set; }


        /// <summary>
        /// день за которое списание
        /// </summary>
        public DateTime DayOfLog { get; set; }
        public DateTime CreationTime { get; set; }


        public long WorkTaskId { get; set; }
        public WorkTask WorkTask { get; set; }


        public long ProjectUserId { get; set; }
        public ProjectUser ProjectUser { get; set; }


        public byte[] RowVersion { get; set; }

    }
}
