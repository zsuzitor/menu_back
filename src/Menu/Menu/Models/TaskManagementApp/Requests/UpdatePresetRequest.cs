using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class UpdatePresetRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }


        public long? CreatorId { get; set; }
        public long? ExecutorId { get; set; }

        public long? StatusId { get; set; }

        public long? SprintId { get; set; }


        public List<long> Labels { get; set; }

        public UpdatePresetRequest()
        {
            Labels = new List<long>();
        }
    }
}
