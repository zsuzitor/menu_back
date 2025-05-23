using BO.Models.CodeReviewApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface ITaskStatusRepository : IGeneralRepository<TaskReviewStatus, long>
    {
        Task<List<TaskReviewStatus>> GetForProjectAsync(long projectId);
    }
}
