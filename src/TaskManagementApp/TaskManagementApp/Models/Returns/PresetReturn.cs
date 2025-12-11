using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.Returns
{
    public class PresetReturn
    {

        public long Id { get; set; }
        public string Name { get; set; }

        public long ProjectId { get; set; }


        public long? CreatorId { get; set; }
        public long? ExecutorId { get; set; }

        public long? StatusId { get; set; }

        public long? SprintId { get; set; }


        public List<long> Labels { get; set; }


        public PresetReturn(Preset obj)
        {
            Id = obj.Id;
            Name = obj.Name;
        }
    }
}
