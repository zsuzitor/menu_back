

using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IWorkTaskStatusService
    {

        Task<List<WorkTaskStatus>> GetStatusesAccessAsync(long projectId, UserInfo userInfo);
        Task<List<WorkTaskStatus>> GetStatusesAsync(long projectId);

        Task<WorkTaskStatus> CreateStatusAsync(string status, long projectId, UserInfo userInfo);
        Task<WorkTaskStatus> DeleteStatusAsync(long statusId, UserInfo userInfo);
        Task<WorkTaskStatus> UpdateStatusAsync(long statusId, string status, UserInfo userInfo);
    }
}
