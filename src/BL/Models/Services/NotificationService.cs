using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    //наверное можно на стратегию переписать, GetNotificationService().WithEmail() - подложит нужную реализацию
    public abstract class NotificationService : INotificationService
    {

        protected readonly INotificationRepository _notificationRepository;
        protected readonly IDateTimeProvider _dateTimeProvider;

        public NotificationService(INotificationRepository notificationRepository, IDateTimeProvider dateTimeProvider)
        {
            _notificationRepository = notificationRepository;
            _dateTimeProvider = dateTimeProvider;
        }



        public async Task<long> CreateAndSendNotification(Notification notification)
        {

            _ = await CreateNotification(notification);

            await TrySend(notification);
            return notification.Id;
        }

        public async Task<long> CreateNotification(Notification notification)
        {
            return (await _notificationRepository.AddAsync(notification)).Id;
        }

        public async Task ReSendAsync(long notificationId)
        {
            var localForSend = await _notificationRepository.GetAsync(notificationId);
            await TrySend(localForSend);

        }


        public async Task<List<long>> CreateNotification(List<Notification> notification)
        {
            await _notificationRepository.AddAsync(notification);
            return notification.Select(x => x.Id).ToList();
        }

        public virtual void SendQueue()
        {
            this.SendQueueAsync().GetAwaiter().GetResult();
        }

        public async Task SendQueueAsync()
        {
            var records = await GetNotificationsForSend();

            await SendQueueAsync(records);

        }

        protected async Task SendQueueAsync(List<Notification> localForSend)
        {

            if (localForSend.Count == 0)
            {
                return;
            }

            var errors = await Send(localForSend);
            foreach (var mail in localForSend)
            {
                mail.SendTryCount = mail.SendTryCount + 1;
                if (!errors.Any(x => x == mail.Id))
                {
                    mail.SendedDate = _dateTimeProvider.CurrentDateTime();
                }
            }

            //localForSend.ForEach(x => {
            //    x.SendedDate = _dateTimeProvider.CurrentDateTime();
            //});
            await _notificationRepository.UpdateAsync(localForSend);
        }




        private async Task<bool> TrySend(Notification rec)
        {
            try
            {
                var complete = await Send(rec);
                rec.SendTryCount = rec.SendTryCount + 1;
                if (complete)
                {
                    rec.SendedDate = _dateTimeProvider.CurrentDateTime();
                }
            }
            catch
            {
                //rec.SendTryCount = rec.SendTryCount + 1;//хз надо или нет
            }

            await _notificationRepository.UpdateAsync(rec);
            return rec.SendedDate != null;
        }


        protected abstract Task<bool> Send(Notification rec);
        protected abstract Task<List<long>> Send(List<Notification> rec);
        protected abstract Task<List<Notification>> GetNotificationsForSend();




    }
}
