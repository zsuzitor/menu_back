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
        Task<WorkTaskLabel> Create(WorkTaskLabel req, UserInfo userInfo);
        Task<WorkTaskLabel> Update(WorkTaskLabel req, UserInfo userInfo);
        Task<List<WorkTaskLabel>> Get(long projectId, UserInfo userInfo);
        Task<List<WorkTaskLabel>> Get(long projectId);
        Task<bool> Delete(long id, UserInfo userInfo);
        Task<bool> AddToTask(long labelId, long taskId, UserInfo userInfo);
        Task<bool> RemoveFromTask(long labelId, long taskId, UserInfo userInfo);
        Task<bool> UpdateTaskLabels(List<long> labelId, long taskId, UserInfo userInfo);
    }
}
