using BL.Models.Services.Interfaces;
using BO.Models;
using BO.Models.Configs;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Models.Services
{


    //todo избавиться от абстракции и переопределения?
    public abstract class EmailServiceBase : NotificationService, IEmailService
    {
        public class Changes
        {
            public string PropName { get; set; }
            public string PropPrevValue { get; set; }
            public string PropNewValue { get; set; }
        }

        //public abstract string ConfigurationKey { get; }

        //public abstract string DefaultSubject { get; }

        protected readonly IEmailServiceSender _emailService;
        protected abstract MailSendingInstanceConfig _config { get; }
        protected abstract string Group { get; }



        public EmailServiceBase(IEmailServiceSender emailService, INotificationRepository notificationRepository, IDateTimeProvider dateTimeProvider)
            : base(notificationRepository, dateTimeProvider)
        {
            _emailService = emailService;
        }




        public virtual async Task<long> SendEmailAsync(string email, string subject, string message)
        {
            return await SendEmailAsync(email, subject, message, Group);
        }

        public virtual async Task<long> QueueEmailAsync(string email, string subject, string message)
        {
            return await QueueEmailAsync(email, subject, message, Group);
        }





        public virtual async Task<List<long>> QueueEmailAsync(List<string> email, string subject, string message)
        {
            return await QueueEmailAsync(email, subject, message, Group);
        }


        protected override async Task<List<Notification>> GetNotificationsForSend()
        {
            return await GetNotificationsForSend(Group);
        }

        protected async Task<List<Notification>> GetNotificationsForSend(string group)
        {

            return await _notificationRepository.GetActual(
                NotificationType.Email, group);
        }

        protected async Task<long> SendEmailAsync(string email, string subject, string message, string group)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return 0;
            }
            var notification = new Notification()
            {
                CreatedDate = _dateTimeProvider.CurrentDateTime(),
                Email = email,
                Message = message,
                Subject = subject,
                Type = NotificationType.Email,
                Group = group,
                SendTryCount = 0,
            };
            return await CreateAndSendNotification(notification);

        }




        protected async Task<long> QueueEmailAsync(string email, string subject, string message, string group)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return 0;
            }
            return await CreateNotification(new Notification()
            {
                CreatedDate = _dateTimeProvider.CurrentDateTime(),
                Email = email,
                Message = message,
                Subject = subject,
                Type = NotificationType.Email,
                Group = group,
            });
        }


        protected async Task<List<long>> QueueEmailAsync(List<string> email, string subject, string message, string group)
        {
            var mails = email.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x =>
            new Notification()
            {
                CreatedDate = _dateTimeProvider.CurrentDateTime(),
                Email = x,
                Message = message,
                Subject = subject,
                Type = NotificationType.Email,
                Group = group,
                SendTryCount = 0,
            }).ToList();
            return await CreateNotification(mails);
        }



        protected override async Task<List<long>> Send(List<Notification> rec)
        {
            var combinedForSend = rec.Select(x => new OneMail()
            {
                Email = x.Email,
                Subject = x.Subject,
                Message = x.Message,
                Id = x.Id,
            }).ToList();
            return await _emailService.SendEmailAsync(combinedForSend, _config);
        }

        protected override async Task<bool> Send(Notification rec)
        {
            var combinedForSend = new OneMail()
            {
                Email = rec.Email,
                Subject = rec.Subject,
                Message = rec.Message,
                Id = rec.Id,
            };
            return await _emailService.SendEmailAsync(combinedForSend, _config);
        }


    }
}
