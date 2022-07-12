

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO.Models.DAL.Domain
{
    public sealed class MainNLogEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        public long Id { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string LogDate { get; set; }
        public string LogLevel { get; set; }
        public string LogLogger { get; set; }
        public string LogMessage { get; set; }
        public string LogException { get; set; }
        public string LogStacktrace { get; set; }
        public string ActionDateStart { get; set; }
        public string ActionDateEnd { get; set; }


        public string Action { get; set; }
        public string ConnectionId { get; set; }
        public string GroupName { get; set; }
        public string UserId { get; set; }

        

    }
}
