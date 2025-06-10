using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface ISprintService
    {
        Task<WorkTaskSprint> Create(long projectId, string name, UserInfo userInfo);
    }
}
