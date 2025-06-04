using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IWorkTimeLogService
    {
        Task<WorkTimeLog> CreateAsync(WorkTimeLog obj, UserInfo userInfo);
        Task<WorkTimeLog> GetAsync(long id, UserInfo userInfo);
        Task<WorkTimeLog> EditAsync(WorkTimeLog obj, UserInfo userInfo);
        Task<WorkTimeLog> DeleteAsync(long id, UserInfo userInfo);

        Task<List<WorkTimeLog>> GetTimeForTaskAsync(long taskId, UserInfo userInfo);
        Task<List<WorkTimeLog>> GetTimeForProjectAsync(long projectId, DateTime startDate, DateTime endDate, UserInfo userInfo, long? userId);
        Task<List<WorkTimeLog>> GetTimeForUserAsync(long userId, DateTime startDate, DateTime endDate, UserInfo userInfo);
    }
}
