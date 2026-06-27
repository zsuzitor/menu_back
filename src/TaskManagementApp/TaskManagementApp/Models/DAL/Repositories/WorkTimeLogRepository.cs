using BO.Models.TaskManagementApp.DAL;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    internal class WorkTimeLogRepository : GeneralRepository<WorkTimeLog, long>, IWorkTimeLogRepository
    {
        private readonly ITasksManagmentAuthRepository _auth;

        public WorkTimeLogRepository(MenuDbContext db, IGeneralRepositoryStrategy repo, ITasksManagmentAuthRepository auth) : base(db, repo)
        {
            _auth = auth;
        }

        public async Task<WorkTimeLog> GetTimeAccessEditAsync(long id, long userId)
        {

            var result = await _db.TaskManagementWorkTimeLog//.Include(x=>x.WorkTask).ThenInclude(x=>x.Project)
                .Where(x => x.Id == id && x.UserId == userId)
                .Select(x => x).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }

            var access = await _auth.CanAccessTask(result.WorkTaskId, userId);
            if (!access)
                return null;

            return result;
        }

        public async Task<List<WorkTimeLog>> GetTimeForOneUserProjectAsync(long projectId, DateTime startDate, DateTime endDate, long userId)
        {
            //_db.TaskManagementProjectUsers.Where(u => u.ProjectId == projectId && u.MainAppUserId == userInfo.UserId).Include(x=>x.);
            var access = await _auth.CanAccessProject(projectId, userId);
            if(!access)
                return new List<WorkTimeLog>();

            return await _db.TaskManagementWorkTimeLog.AsNoTracking()
                .Include(x => x.WorkTask)
                .Where(x => x.WorkTask.ProjectId == projectId
                && x.DayOfLog.Date >= startDate.Date && x.DayOfLog.Date <= endDate.Date).Select(x => x).ToListAsync();

            //_db.TaskManagementWorkTimeLog
            //_db.TaskManagementTasks

            //_db.TaskManagementTasks.Where(x => x.ProjectId == projectId)
            //    .Join(_db.TaskManagementTaskProject, x => x.ProjectId, x => x.Id
            //    , (x, y) => new { proj = y, tsk = x }).Join(_db.TaskManagementProjectUsers, x => x.proj.ProjectId, x => x.Id
            //    , (x, y) => new { proj = x, tsk = y });
        }

        public async Task<List<WorkTimeLog>> GetTimeForOneUserTaskAsync(long taskId, long userId)
        {
            var access = await _auth.CanAccessTask(taskId, userId);
            if (!access)
                return new List<WorkTimeLog>();

            return await _db.TaskManagementWorkTimeLog.AsNoTracking().ToListAsync();

        }

        public async Task<List<WorkTimeLog>> GetTimeForProjectAsync(long projectId, DateTime startDate, DateTime endDate, long? userId)
        {
            return await _db.TaskManagementWorkTimeLog.AsNoTracking()
                .Include(x => x.WorkTask)
                .Where(x => x.WorkTask.ProjectId == projectId && (userId == null || x.UserId == userId)
                 && x.DayOfLog.Date >= startDate.Date && x.DayOfLog.Date <= endDate.Date).Select(x => x).ToListAsync();
        }

        public async Task<List<WorkTimeLog>> GetTimeForTaskAsync(long taskId)
        {
            return await _db.TaskManagementWorkTimeLog.AsNoTracking()
                .Where(x => x.WorkTaskId == taskId).ToListAsync();

        }

        public async Task<List<WorkTimeLog>> GetTimeForUserAsync(long userId, DateTime startDate, DateTime endDate)
        {

            var result =  await _db.TaskManagementWorkTimeLog.AsNoTracking()
                .Where(x =>
                 (x.UserId == userId)
                && x.DayOfLog.Date >= startDate.Date && x.DayOfLog.Date <= endDate.Date).Select(x => x).ToListAsync();

            return result;
            //мне кажется то что тут можно посмотреть свои списания даже из проекта из которого тебя выгнали вцелом норм, редачить ты их не сможешь

            //var taskId = result.Select(x => x.WorkTaskId).Distinct();
            //foreach(var tId in taskId)
            //{

            //    var access = await _auth.CanAccessTask(tId, userId);
            //    if (!access.access)
            //}
        }
    }
}
