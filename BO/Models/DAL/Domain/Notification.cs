using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.DAL.Domain
{
    public enum NotificationType
    {
        Email = 1,
        SMS
    }

    public class Notification : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        /// <summary>
        /// для разделения например по приложениям
        /// </summary>
        public string Group { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SendedDate { get; set; }


    }
}
