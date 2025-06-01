using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface ITaskStatusRepository : IGeneralRepository<WorkTaskStatus, long>
    {
        Task<List<WorkTaskStatus>> GetForProjectAsync(long projectId);
    }
}
