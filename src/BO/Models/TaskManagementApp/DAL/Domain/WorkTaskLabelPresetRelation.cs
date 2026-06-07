using BO.Models.DAL;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class WorkTaskLabelPresetRelation : IDomainRecord<long>
    {
        public long Id { get; set; }


        public long PresetId { get; set; }
        public Preset Preset { get; set; }

        public long LabelId { get; set; }
        public WorkTaskLabel Label { get; set; }
    }
}
