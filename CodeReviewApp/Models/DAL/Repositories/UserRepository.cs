
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
    internal class UserRepository : GeneralRepository<ProjectUser, long>, IProjectUserRepository
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
            return await _db.ReviewProjectUsers.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<ProjectUser> GetByMainAppUserIdAsync(long projectId, long mainAppUserId)
        {
            return await _db.ReviewProjectUsers
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.MainAppUserId == mainAppUserId);
        }
    }
}
