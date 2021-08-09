

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO.Models.DAL.Domain
{
    public class MainNLogEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        public Guid Id { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string LogDate { get; set; }
        public string LogLevel { get; set; }
        public string LogLogger { get; set; }
        public string LogMessage { get; set; }
        public string LogException { get; set; }
        public string LogStacktrace { get; set; }


    }
}
