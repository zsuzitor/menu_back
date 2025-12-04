using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IWorkTaskLabelRepository : IGeneralRepository<WorkTaskLabel, long>
    {
        Task<WorkTaskLabelTaskRelation> CreateAsync(WorkTaskLabelTaskRelation obj);
        Task<List<WorkTaskLabelTaskRelation>> DeleteAsync(List<WorkTaskLabelTaskRelation> list);
        Task<bool> ExistsAsync(long labelId, long taskId);
        Task<bool> RemoveFromTaskIdExistAsync(long labelId, long taskId);
        Task<List<WorkTaskLabel>> GetForTaskAsync(long taskId);
        Task<List<WorkTaskLabel>> GetForProjectAsync(long projectId);
        Task<bool> ExistsAsync(string name, long projectId);
    }
}
