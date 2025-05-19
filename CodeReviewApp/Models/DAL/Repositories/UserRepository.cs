
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories
{
    internal sealed class UserRepository : GeneralRepository<ProjectUser, long>, IProjectUserRepository
    {
        public UserRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<ProjectUser> CreateAsync(ProjectUser user)
        {
            return await base.AddAsync(user);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAsync(long projectId)
        {
            return await _db.ReviewProjectUsers.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<ProjectUser> GetByMainAppUserIdAsync(long mainAppUserId, long projectId)
        {
            return await _db.ReviewProjectUsers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.MainAppUserId == mainAppUserId);
        }

        public async Task<long?> GetIdByMainAppIdAsync(long userId, long projectId)
        {
            return (await _db.ReviewProjectUsers.Select(x => new {x.Id, x.MainAppUserId, x.ProjectId, x.Deactivated })
                .FirstOrDefaultAsync(x => x.MainAppUserId == userId && x.ProjectId == projectId && !x.Deactivated))?.Id;
        }

        public async Task<string> GetNotificationEmailAsync(long userId)
        {
            return (await _db.ReviewProjectUsers.Select(x => new { x.Id, x.NotifyEmail })
                .FirstOrDefaultAsync(x => x.Id == userId))?.NotifyEmail;
        }

        public async Task<(string email, long? mainAppId)> GetNotificationEmailWithMainAppIdAsync(long userId)
        {
            var res =  (await _db.ReviewProjectUsers.Select(x => new { x.Id, x.NotifyEmail, x.MainAppUserId })
                   .FirstOrDefaultAsync(x => x.Id == userId));

            return (res?.NotifyEmail, res?.MainAppUserId);
        }

        public override async Task<ProjectUser> DeleteAsync(ProjectUser user)
        {
            _db.ReviewProjectUsers.Attach(user);
            user.Deactivated = true;
            await _db.SaveChangesAsync();
            return user;
        }

        public override async Task<IEnumerable<ProjectUser>> DeleteAsync(IEnumerable<ProjectUser> records)
        {
            _db.ReviewProjectUsers.AttachRange(records);
            foreach (var item in records)
            {
                item.Deactivated = true;
            }

            await _db.SaveChangesAsync();
            return records;
        }
        public override async Task<ProjectUser> DeleteAsync(long recordId)
        {
            var record = await GetAsync(recordId);
            if (record != null)
            {
                record.Deactivated = false;
                await _db.SaveChangesAsync();
            }

            return record;
        }

        public async Task<bool> ExistAsync(long projectId, long userId)
        {
            return await _db.ReviewProjectUsers.AnyAsync(x => x.Id == userId && x.ProjectId == projectId);
        }

        public async Task<bool> ExistByMainIdAsync(long projectId, long mainAppUserId)
        {
            return await _db.ReviewProjectUsers.AnyAsync(x => x.MainAppUserId == mainAppUserId && x.ProjectId == projectId);

        }
    }
}
