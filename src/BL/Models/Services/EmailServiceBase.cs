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
        private readonly IDateTimeProvider _dateTimeProvider;
        protected abstract MailSendingInstanceConfig _config { get; }
        protected abstract string Group { get; }



        public EmailServiceBase(IEmailServiceSender emailService, INotificationRepository notificationRepository, IDateTimeProvider dateTimeProvider)
        //IConfiguration configuration)
        {
            _emailService = emailService;
            _notificationRepository = notificationRepository;
            _dateTimeProvider = dateTimeProvider;
        }


        public async Task ReSendEmailAsync(long notificationId)
        {
            var localForSend = await _notificationRepository.GetAsync(notificationId);
            await TrySend(localForSend);
        }

        public virtual async Task<long> SendEmailAsync(string email, string subject, string message)
        {
            return await SendEmailAsync(email, subject, message, Group);
        }

        public virtual async Task<long> QueueEmailAsync(string email, string subject, string message)
        {
            return await QueueEmailAsync(email, subject, message, Group);
        }


        public virtual async Task SendQueueAsync()
        {
            var localForSend = await _notificationRepository.GetActual(
                BO.Models.DAL.Domain.NotificationType.Email, Group);

            await SendQueueAsync(localForSend);

        }

        public virtual async Task<List<long>> QueueEmailAsync(List<string> email, string subject, string message)
        {
            return await QueueEmailAsync(email, subject, message, Group);
        }


        protected async Task<long> SendEmailAsync(string email, string subject, string message, string group)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return 0;
            }

            var rec = await _notificationRepository.AddAsync(new Notification()
            {
                CreatedDate = _dateTimeProvider.CurrentDateTime(),
                Email = email,
                Message = message,
                Subject = subject,
                Type = NotificationType.Email,
                Group = group,
                SendTryCount = 0,
            });

            await TrySend(rec);
            return rec.Id;
        }



        private async Task TrySend(Notification rec)
        {
            try
            {
                var complete = await _emailService.SendEmailAsync(rec.Email, rec.Subject, rec.Message, _config);
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
        }




        protected async Task<long> QueueEmailAsync(string email, string subject, string message, string group)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return 0;
            }

            var res = await _notificationRepository.AddAsync(new BO.Models.DAL.Domain.Notification()
            {
                CreatedDate = _dateTimeProvider.CurrentDateTime(),
                Email = email,
                Message = message,
                Subject = subject,
                Type = BO.Models.DAL.Domain.NotificationType.Email,
                Group = group,
            });

            return res?.Id ?? 0;
        }


        protected async Task<List<long>> QueueEmailAsync(List<string> email, string subject, string message, string group)
        {
            var mails = email.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x =>
            new BO.Models.DAL.Domain.Notification()
            {
                CreatedDate = _dateTimeProvider.CurrentDateTime(),
                Email = x,
                Message = message,
                Subject = subject,
                Type = BO.Models.DAL.Domain.NotificationType.Email,
                Group = group,
                SendTryCount = 0,
            }).ToList();

            await _notificationRepository.AddAsync(mails);
            return mails.Select(x => x.Id).ToList();
        }




        protected async Task SendQueueAsync(List<Notification> localForSend)
        {

            if (localForSend.Count == 0)
            {
                return;
            }

            var combinedForSend = localForSend.Select(x => new OneMail()
            {
                Email = x.Email,
                Subject = x.Subject,
                Message = x.Message,
                Id = x.Id,
            }).ToList();
            //var combinedForSend = new List<OneMail>();
            //var groupedByEmail = localForSend.GroupBy(x => x.Email);
            //foreach (var groupByMail in groupedByEmail)
            //{
            //    var groupedBySubject = groupByMail.ToList().GroupBy(x => x.Subject);
            //    foreach (var groupBySubject in groupedBySubject)
            //    {
            //        var messagesBody = groupBySubject.ToList().Select(x => x.Message).Distinct();

            //        combinedForSend.Add(new OneMail()
            //        {
            //            Email = groupByMail.Key,
            //            Subject = groupBySubject.Key,
            //            Message = string.Join('\n', messagesBody),
            //        });
            //    }
            //}

            var errors = await _emailService.SendEmailAsync(combinedForSend, _config);
            foreach (var mail in localForSend)
            {
                mail.SendTryCount = mail.SendTryCount + 1;
                if (!errors.Any(x=>x == mail.Id))
                {
                    mail.SendedDate = _dateTimeProvider.CurrentDateTime();
                }
            }

            //localForSend.ForEach(x => {
            //    x.SendedDate = _dateTimeProvider.CurrentDateTime();
            //});
            await _notificationRepository.UpdateAsync(localForSend);
        }



    }
}
