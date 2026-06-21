
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
        Task<(bool access, bool isAdmin)> CanAccessProject(long projectId, long userId);
        Task<bool> CanEditTask(long taskId, long userId);
        Expression<Func<ProjectUser, bool>> IsAdmin(long userId);
        Expression<Func<ProjectUser, bool>> IsAccess(long userId);


    }
}
