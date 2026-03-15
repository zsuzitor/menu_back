using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface ILabelService
    {
        Task<WorkTaskLabel> Create(WorkTaskLabel req, long userId);
        Task<WorkTaskLabel> Update(WorkTaskLabel req, long userId);
        Task<List<WorkTaskLabel>> Get(long projectId, long userId);
        Task<List<WorkTaskLabel>> Get(long projectId);
        Task<bool> Delete(long id, long userId);
        Task<bool> AddToTask(long labelId, long taskId, long userId);
        Task<bool> RemoveFromTask(long labelId, long taskId, long userId);
        Task<bool> UpdateTaskLabels(List<long> labelId, long taskId, long userId);
    }
}
