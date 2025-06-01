using BL.Models.Services.Interfaces;
using BO.Models;
using BO.Models.Configs;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    

    //todo избавиться от абстракции и переопределения?
    public abstract class EmailServiceBase : IEmailService
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
        protected readonly INotificationRepository _notificationRepository;
        protected abstract MailSendingInstanceConfig _config { get; }
        protected abstract string Group { get; }



        public EmailServiceBase(IEmailServiceSender emailService, INotificationRepository notificationRepository)
        //IConfiguration configuration)
        {
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }

        public virtual async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }


            var rec = await _notificationRepository.AddAsync(new BO.Models.DAL.Domain.Notification()
            {
                CreatedDate = System.DateTime.Now,
                Email = email,
                Message = message,
                Subject = subject,
                Type = BO.Models.DAL.Domain.NotificationType.Email,
                Group = Group,
            });

            await _emailService.SendEmailAsync(email, subject, message, _config);

            rec.SendedDate = System.DateTime.Now;
            await _notificationRepository.UpdateAsync(rec);
        }

        public virtual async Task QueueEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            await _notificationRepository.AddAsync(new BO.Models.DAL.Domain.Notification()
            {
                CreatedDate = System.DateTime.Now,
                Email = email,
                Message = message,
                Subject = subject,
                Type = BO.Models.DAL.Domain.NotificationType.Email,
                Group = Group,
            });
        }

        public virtual async Task QueueEmailAsync(List<string> email, string subject, string message)
        {
            var mails = email.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => 
            new BO.Models.DAL.Domain.Notification()
            {
                CreatedDate = System.DateTime.Now,
                Email = x,
                Message = message,
                Subject = subject,
                Type = BO.Models.DAL.Domain.NotificationType.Email,
                Group = Group,
            }).ToList();

            await _notificationRepository.AddAsync(mails);
        }

        public virtual async Task SendQueueAsync()
        {
            var localForSend = await _notificationRepository.GetActual(
                BO.Models.DAL.Domain.NotificationType.Email, Group);

            if (localForSend.Count == 0)
            {
                return;
            }

            var combinedForSend = new List<OneMail>();
            var groupedByEmail = localForSend.GroupBy(x => x.Email);
            foreach (var groupByMail in groupedByEmail)
            {
                var groupedBySubject = groupByMail.ToList().GroupBy(x => x.Subject);
                foreach (var groupBySubject in groupedBySubject)
                {
                    var messagesBody = groupBySubject.ToList().Select(x => x.Message).Distinct();

                    combinedForSend.Add(new OneMail()
                    {
                        Email = groupByMail.Key,
                        Subject = groupBySubject.Key,
                        Message = string.Join('\n', messagesBody),
                    });
                }
            }

            await _emailService.SendEmailAsync(combinedForSend, _config);
            localForSend.ForEach(x => x.SendedDate = System.DateTime.Now);
            await _notificationRepository.UpdateAsync(localForSend);
        }
    }
}
