﻿
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
    public class ProjectRepository : GeneralRepository<Project, long>, IProjectRepository
    {
        public ProjectRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<Project> CreateAsync(string name, ProjectUser user)
        {
            var newProject = new Project() { Name = name, Users = new List<ProjectUser>() { user } };
            return await base.AddAsync(newProject);
        }

        public async Task<bool> ExistIfAccessAsync(long id, long mainAppUserId)
        {
            return await _db.ReviewProject.Include(x => x.Users)
                .Where(x => x.Id == id
                && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId) != null).FirstOrDefaultAsync() != null;
        }

       

        public async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {
            return await _db.ReviewProject.Include(x => x.Users)
                            .Where(x => x.Id == id
                            && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId) != null).FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            return await _db.ReviewProjectUsers.Where(x => x.MainAppUserId == userId)
                .Include(x => x.Project).Select(x=>x.Project).ToListAsync();

                //.Join(_db.ReviewProject,u=>u.ProjectId)

        }
    }
}