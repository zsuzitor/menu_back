using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial.Arenas;

namespace TaskManagementApp.Models.DAL.Repositories
{
    internal class WorkTimeLogRepository : GeneralRepository<WorkTimeLog, long>, IWorkTimeLogRepository
    {
        public WorkTimeLogRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public Task<WorkTimeLog> GetTimeAccessAsync(long id, UserInfo userInfo)
        {
            return _db.TaskManagementWorkTimeLog//.Include(x=>x.WorkTask).ThenInclude(x=>x.Project)
                .Include(x => x.ProjectUser)
                .Where(x => x.ProjectUser.MainAppUserId == userInfo.UserId && !x.ProjectUser.Deactivated && x.Id == id)
                .Select(x => x).FirstOrDefaultAsync();
        }

        public async Task<List<WorkTimeLog>> GetTimeForOneUserProjectAsync(long projectId, DateTime startDate, DateTime endDate, UserInfo userInfo)
        {
            //_db.TaskManagementProjectUsers.Where(u => u.ProjectId == projectId && u.MainAppUserId == userInfo.UserId).Include(x=>x.);

            return await _db.TaskManagementWorkTimeLog.AsNoTracking().Include(x => x.ProjectUser)
                .Include(x => x.WorkTask)
                .Where(x => x.WorkTask.ProjectId == projectId
                && x.ProjectUser.MainAppUserId == userInfo.UserId && !x.ProjectUser.Deactivated
                && x.DayOfLog >= startDate && x.DayOfLog <= endDate).Select(x => x).ToListAsync();

            //_db.TaskManagementWorkTimeLog
            //_db.TaskManagementTasks

            //_db.TaskManagementTasks.Where(x => x.ProjectId == projectId)
            //    .Join(_db.TaskManagementTaskProject, x => x.ProjectId, x => x.Id
            //    , (x, y) => new { proj = y, tsk = x }).Join(_db.TaskManagementProjectUsers, x => x.proj.ProjectId, x => x.Id
            //    , (x, y) => new { proj = x, tsk = y });
        }

        public async Task<List<WorkTimeLog>> GetTimeForOneUserTaskAsync(long taskId, UserInfo userInfo)
        {
            return await _db.TaskManagementWorkTimeLog.AsNoTracking().Include(x => x.ProjectUser)
                .Where(x => x.WorkTaskId == taskId
                && x.ProjectUser.MainAppUserId == userInfo.UserId && !x.ProjectUser.Deactivated).Select(x => x).ToListAsync();

        }

        public async Task<List<WorkTimeLog>> GetTimeForProjectAsync(long projectId, DateTime startDate, DateTime endDate, long? userId)
        {

            return await _db.TaskManagementWorkTimeLog.AsNoTracking()
                .Include(x => x.WorkTask).Include(x => x.ProjectUser)//.ThenInclude(x=>x.Project).ThenInclude(x=>x.Users)
                .Where(x => x.WorkTask.ProjectId == projectId && (userId == null || x.ProjectUser.Id == userId)
                 && x.DayOfLog >= startDate && x.DayOfLog <= endDate).Select(x => x).ToListAsync();
        }

        public async Task<List<WorkTimeLog>> GetTimeForTaskAsync(long taskId)
        {
            return await _db.TaskManagementWorkTimeLog.AsNoTracking()
                .Where(x => x.WorkTaskId == taskId).ToListAsync();

        }

        public async Task<List<WorkTimeLog>> GetTimeForUserAsync(long? userId, DateTime startDate, DateTime endDate, UserInfo userInfo)
        {

            return await _db.TaskManagementWorkTimeLog.AsNoTracking().Include(x => x.ProjectUser)
                .Where(x =>
                 ((userId != null && x.ProjectUserId == userId) || (userId == null && x.ProjectUser.MainAppUserId == userInfo.UserId))
                    && !x.ProjectUser.Deactivated
                && x.DayOfLog >= startDate && x.DayOfLog <= endDate).Select(x => x).ToListAsync();

        }
    }
}
