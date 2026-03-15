

using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IWorkTaskStatusService
    {

        Task<List<WorkTaskStatus>> GetStatusesAccessAsync(long projectId, long userId);
        Task<List<WorkTaskStatus>> GetStatusesAsync(long projectId);

        Task<WorkTaskStatus> CreateStatusAsync(string status, long projectId, long userId);
        Task<WorkTaskStatus> DeleteStatusAsync(long statusId, long userId);
        Task<WorkTaskStatus> UpdateStatusAsync(long statusId, string status, long userId);
    }
}
