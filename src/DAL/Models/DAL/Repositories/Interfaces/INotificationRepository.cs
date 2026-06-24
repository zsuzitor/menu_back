using BO.Models.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface INotificationRepository : IGeneralRepository<Notification, long>
    {
        Task<List<Notification>> GetActual(NotificationType type, string group);
    }
}
