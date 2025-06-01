
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IWorkTaskCommentRepository : IGeneralRepository<WorkTaskComment, long>
    {
        Task<WorkTaskComment> DeleteAsync(long id, long userId);
        Task<WorkTaskComment> UpdateAsync(long id, long userId, string text);
    }
}
