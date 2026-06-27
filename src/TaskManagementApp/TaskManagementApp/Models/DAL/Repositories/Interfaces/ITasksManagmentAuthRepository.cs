
using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface ITasksManagmentAuthCachedRepository : ITasksManagmentAuthRepository
    {

    }
    public interface ITasksManagmentAuthRepository
    {
        Task<bool> CanEditProject(long projectId, long userId);
        Task<bool> CanAdminEditProject(long projectId, long userId);
        Task<bool> CanViewProject(long projectId, long userId);
        Task<bool> CanAccessProject(long projectId, long userId);
        Task<bool> CanAccessTask(long taskId, long userId);
        Task<bool> CanViewTask(long taskId, long userId);
        Task<bool> CanEditTask(long taskId, long userId);
        Expression<Func<ProjectUser, bool>> IsAdmin(long userId);
        Expression<Func<ProjectUser, bool>> IsAccess(long userId);
        Expression<Func<ProjectUser, bool>> IsAccess(long userId, long projectId);


    }
}
