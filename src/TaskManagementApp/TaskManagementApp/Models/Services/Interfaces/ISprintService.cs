using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface ISprintService
    {
        Task<ProjectSprint> Create(ProjectSprint req, long userId);
        Task<ProjectSprint> Update(ProjectSprint req, long userId);
        Task<ProjectSprint> Get(long sprintId, long userId);
        Task<List<WorkTask>> GetTasks(long sprintId, long userId);
        Task<List<ProjectSprint>> GetForProject(long projectId);
        Task<List<ProjectSprint>> GetForProjectWithRights(long projectId, long userId);
        Task<ProjectSprint> Delete(long id, long userId);
        Task<bool> AddTaskToSprint(long sprintId, long taskId, long userId);
        Task<bool> UpdateTaskSprints(List<long> sprintId, long taskId, long userId);
        Task<bool> DeleteTaskFromSprint(long sprintId, long taskId, long userId);
    }
}
