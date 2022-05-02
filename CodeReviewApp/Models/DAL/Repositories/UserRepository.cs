﻿
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories
{
    internal class UserRepository : GeneralRepository<ProjectUser, long>, IProjectUserRepository
    {
        public UserRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<ProjectUser> Create(ProjectUser user)
        {
            return await base.AddAsync(user);
        }
    }
}