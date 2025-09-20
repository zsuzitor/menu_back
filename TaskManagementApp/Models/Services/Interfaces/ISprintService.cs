using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface ISprintService
    {
        Task<ProjectSprint> Create(ProjectSprint req, UserInfo userInfo);
        Task<ProjectSprint> Get(long sprintId, UserInfo userInfo);
        Task<List<WorkTask>> GetTasks(long sprintId, UserInfo userInfo);
        Task<List<ProjectSprint>> GetForProject(long projectId, UserInfo userInfo);
        Task<List<ProjectSprint>> GetForProjectWithRights(long projectId, UserInfo userInfo);
        Task<ProjectSprint> Delete(long id, UserInfo userInfo);
        Task<bool> AddTaskToSprint(long sprintId, long taskId, UserInfo userInfo);
        Task<bool> UpdateTaskSprints(List<long> sprintId, long taskId, UserInfo userInfo);
        Task<bool> DeleteTaskFromSprint(long sprintId, long taskId, UserInfo userInfo);
    }
}
