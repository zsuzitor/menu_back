using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IWorkTimeLogRepository : IGeneralRepository<WorkTimeLog, long>
    {
        Task<List<WorkTimeLog>> GetTimeForTaskAsync(long taskId, UserInfo userInfo);
        Task<List<WorkTimeLog>> GetTimeForProjectAsync(long projectId, DateTime startDate, DateTime endDate, UserInfo userInfo);
        Task<List<WorkTimeLog>> GetTimeForUserAsync(long userId, DateTime startDate, DateTime endDate, UserInfo userInfo);
    }
}
