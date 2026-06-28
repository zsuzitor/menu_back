
using BO.Models.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    internal interface INotificationService
    {
        Task<long> CreateNotification(Notification notification);
        Task<List<long>> CreateNotification(List<Notification> notification);
        Task<long> CreateAndSendNotification(Notification notification);
        Task ReSendAsync(long notificationId);
        //Task<bool> Send(Notification rec);
        //Task SendQueueAsync(List<Notification> localForSend);
        void SendQueue();
    }
}
