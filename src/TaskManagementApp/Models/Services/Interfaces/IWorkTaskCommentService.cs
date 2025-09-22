
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IWorkTaskCommentService
    {
        /// <summary>
        /// без валидаций
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<WorkTaskComment> CreateAsync(WorkTaskComment comment);
        Task<WorkTaskComment> EditAsync(long commentId, string text, UserInfo userInfo);
        Task<WorkTaskComment> DeleteAsync(long commentId, UserInfo userInfo);

    }
}
