using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class NotificationRepository : GeneralRepository<Notification, long>, INotificationRepository
    {
        public NotificationRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {

        }

        public async Task<List<Notification>> GetActual(NotificationType type, string group)
        {
            return await _db.Notifications.Where(x => x.SendedDate == null
                && x.SendTryCount < 10
                && x.Type == type
                && (x.Group == group || string.IsNullOrWhiteSpace(group)))
                .ToListAsync();
        }

        public async Task<List<Notification>> GetActual(NotificationType type, List<string> excludedGroup)
        {
            excludedGroup = excludedGroup ?? new List<string>();
            return await _db.Notifications.Where(x => x.SendedDate == null
                && x.SendTryCount < 10
                && x.Type == type
                && !excludedGroup.Contains(x.Group))
                .ToListAsync();
        }
    }
}
