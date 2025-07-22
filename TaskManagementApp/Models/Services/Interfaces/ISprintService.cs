using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface ISprintService
    {
        Task<ProjectSprint> Create(long projectId, string name, UserInfo userInfo);
        Task<ProjectSprint> Delete(long id, UserInfo userInfo);
        Task<bool> AddTaskToSprint(long sprintId, long taskId, UserInfo userInfo);
        Task<bool> DeleteTaskFromSprint(long sprintId, long taskId, UserInfo userInfo);
    }
}
