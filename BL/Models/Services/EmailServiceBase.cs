using BL.Models.Services.Interfaces;
using BO.Models;
using BO.Models.Configs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public abstract class EmailServiceBase : IEmailService
    {
        //public abstract string ConfigurationKey { get; }

        //public abstract string DefaultSubject { get; }

        protected readonly IEmailServiceSender _emailService;
        protected abstract MailSendingInstanceConfig _config { get; }

        //todo думаю нужен репозиторий и бд, все взаимодействие можно растащить по методам
        protected readonly List<OneMail> _forSend;
        protected readonly object _lock;

        public EmailServiceBase(IEmailServiceSender emailService)
        //IConfiguration configuration)
        {
            _emailService = emailService;
            _forSend = new List<OneMail>();
            _lock = new object();
        }

        public virtual async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            await _emailService.SendEmailAsync(email, subject, message, _config);
        }

        public virtual async Task QueueEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            lock (_lock)
            {
                _forSend.Add(new OneMail() { Email = email, Subject = subject, Message = message });
            }
        }

        public virtual async Task QueueEmailAsync(List<string> email, string subject, string message)
        {
            lock (_lock)
            {
                foreach (var m in email)
                {
                    if (!string.IsNullOrWhiteSpace(m))
                    {
                        _forSend.Add(new OneMail() { Email = m, Subject = subject, Message = message });
                    }
                }
            }
        }

        public virtual async Task SendQueueAsync()
        {
            List<OneMail> localForSend;
            lock (_lock)
            {
                localForSend = _forSend.Select(x => x).ToList();
                _forSend.Clear();
            }

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
        }
    }
}
