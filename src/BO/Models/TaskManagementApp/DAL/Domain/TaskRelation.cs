using BO.Models.DAL;

namespace BO.Models.TaskManagementApp.DAL.Domain
{
    public class TaskRelation : IDomainRecord<long>
    {
        public long Id { get; set; }

        /// <summary>
        /// главная задача в связке, если допустимо
        /// </summary>
        public long MainWorkTaskId { get; set; }
        public WorkTask MainWorkTask { get; set; }

        /// <summary>
        /// зависимая задача, если допустимо
        /// </summary>
        public long SubWorkTaskId { get; set; }
        public WorkTask SubWorkTask { get; set; }

        public TaskRelationTypeEnum RelationType { get; set; }


        public byte[] RowVersion { get; set; }


    }
}
