using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class DefaultEmailService : EmailServiceBase
    {
        public const string ConfigurationKey = "DefaultMailSettings";

        private readonly MailSendingInstanceConfig __config;
        protected override MailSendingInstanceConfig _config => __config;
        protected override string Group => null;


        public DefaultEmailService(
            IEmailServiceSender emailService, INotificationRepository notificationRepository, MailSendingConfig config, IDateTimeProvider dateTimeProvider)
            : base(emailService, notificationRepository, dateTimeProvider)
        {
            __config = config.Values[ConfigurationKey];
        }


        new public async Task SendEmailAsync(string email, string subject, string message, string group)
        {
            await base.SendEmailAsync(email, subject, message, group);
        }


        new public async Task QueueEmailAsync(List<string> email, string subject, string message, string group)
        {
            await base.QueueEmailAsync(email,subject,message,group);
        }

        new public async Task QueueEmailAsync(string email, string subject, string message, string group)
        {
            await base.QueueEmailAsync(email, subject, message, group);
        }

         public async Task SendQueueAsync(string group)
        {

            var localForSend = await GetNotificationsForSend(group);
            await base.SendQueueAsync(localForSend);
        }



        protected override async Task<List<Notification>> GetNotificationsForSend()
        {
            return await _notificationRepository.GetActual(
                NotificationType.Email, new List<string>() { "AuthEmail" });
        }

    }
}
