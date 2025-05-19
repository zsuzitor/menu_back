using BO.Models;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class NotificationRepository : GeneralRepository<Notification, long>, INotificationRepository
    {
        public NotificationRepository(MenuDbContext db) : base(db)
        {

        }

        public async Task<List<Notification>> GetActual(NotificationType type, string group)
        {
            return await _db.Notifications.Where(x => x.SendedDate == null
                && x.Type == type
                && (x.Group == group || string.IsNullOrWhiteSpace(group)))
                .ToListAsync();
        }


    }
}
