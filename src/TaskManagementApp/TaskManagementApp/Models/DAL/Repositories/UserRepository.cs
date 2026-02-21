
using BO.Models.DAL.Domain;
using BO.Models.TaskManagementApp.DAL;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    internal sealed class UserRepository : GeneralRepository<ProjectUser, long>, IProjectUserRepository
    {
        public UserRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<ProjectUser> CreateAsync(ProjectUser user)
        {
            return await base.AddAsync(user);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAsync(long projectId)
        {
            return await _db.TaskManagementProjectUsers.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<ProjectUser> GetByMainAppUserIdAsync(long mainAppUserId, long projectId)
        {
            return await _db.TaskManagementProjectUsers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.MainAppUserId == mainAppUserId);
        }

        public async Task<long?> GetIdByMainAppIdAsync(long userId, long projectId)
        {
            return (await _db.TaskManagementProjectUsers
                .Where(x => x.MainAppUserId == userId && x.ProjectId == projectId && x.Role != UserRoleEnum.Deactivated)
                .Select(x => new { x.Id, x.MainAppUserId })
                .FirstOrDefaultAsync())?.Id;
        }

        public async Task<string> GetNotificationEmailAsync(long userId)
        {
            return (await _db.TaskManagementProjectUsers.Select(x => new { x.Id, x.NotifyEmail })
                .FirstOrDefaultAsync(x => x.Id == userId))?.NotifyEmail;
        }

        public async Task<(string email, long? mainAppId)> GetNotificationEmailWithMainAppIdAsync(long userId)
        {
            var res = (await _db.TaskManagementProjectUsers.Select(x => new { x.Id, x.NotifyEmail, x.MainAppUserId })
                   .FirstOrDefaultAsync(x => x.Id == userId));

            return (res?.NotifyEmail, res?.MainAppUserId);
        }

        public async Task<List<ProjectUser>> GetProjectUserAsync(long projectId, List<long> usersId)
        {
            return await (_db.TaskManagementProjectUsers.AsNoTracking().Where(x => x.ProjectId == projectId && usersId.Contains(x.Id)).ToListAsync());
        }

        public override async Task<ProjectUser> DeleteAsync(ProjectUser user)
        {
            _db.TaskManagementProjectUsers.Attach(user);
            user.Role = UserRoleEnum.Deactivated;
            await _db.SaveChangesAsync();
            return user;
        }

        public override async Task<IEnumerable<ProjectUser>> DeleteAsync(IEnumerable<ProjectUser> records)
        {
            _db.TaskManagementProjectUsers.AttachRange(records);
            foreach (var item in records)
            {
                item.Role = UserRoleEnum.Deactivated;
            }

            await _db.SaveChangesAsync();
            return records;
        }
        public override async Task<ProjectUser> DeleteAsync(long recordId)
        {
            var record = await GetAsync(recordId);
            if (record != null)
            {
                record.Role = UserRoleEnum.Deactivated;
                await _db.SaveChangesAsync();
            }

            return record;
        }

        public async Task<bool> ExistAsync(long projectId, long userId)
        {
            return await _db.TaskManagementProjectUsers.AnyAsync(x => x.Id == userId && x.ProjectId == projectId);
        }

        public async Task<bool> ExistByMainIdAsync(long projectId, long mainAppUserId)
        {
            return await _db.TaskManagementProjectUsers.AnyAsync(x => x.MainAppUserId == mainAppUserId && x.ProjectId == projectId);

        }
    }
}
