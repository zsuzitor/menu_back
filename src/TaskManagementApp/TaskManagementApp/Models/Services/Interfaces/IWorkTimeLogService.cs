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
        Task<WorkTimeLog> CreateAsync(WorkTimeLog obj, long userId);
        Task<WorkTimeLog> GetAsync(long id, long userId);
        Task<WorkTimeLog> EditAsync(WorkTimeLog obj, long userId);
        Task<WorkTimeLog> DeleteAsync(long id, long userId);

        Task<List<WorkTimeLog>> GetTimeForTaskAsync(long taskId, long userId);
        Task<List<WorkTimeLog>> GetTimeForProjectAsync(long projectId, DateTime startDate, DateTime endDate, long currentUserId, long? userId);
        Task<List<WorkTimeLog>> GetTimeForUserAsync(long? userId, DateTime startDate, DateTime endDate, long currentUserId);
    }
}
