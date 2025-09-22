using BO.Models.TaskManagementApp.DAL.Domain;

namespace TaskManagementApp.Models.Returns
{
    public class WorkTaskStatusReturn
    {

        public long Id { get; set; }
        public string Name { get; set; }

        public WorkTaskStatusReturn(WorkTaskStatus model)
        {
            Id = model.Id;
            Name = model.Name;
        }
    }
}
