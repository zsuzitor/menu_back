
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
            return (await _db.ReviewProjectUsers.Select(x => new {x.Id, x.MainAppUserId, x.ProjectId })
                .FirstOrDefaultAsync(x => x.MainAppUserId == userId && x.ProjectId == projectId))?.Id;
        }

        public async Task<string> GetNotificationEmailAsync(long userId)
        {
            return (await _db.ReviewProjectUsers.Select(x => new { x.Id, x.NotifyEmail, x.ProjectId })
                .FirstOrDefaultAsync(x => x.Id == userId))?.NotifyEmail;
        }
    }
}
