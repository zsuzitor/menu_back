using BL.Models.Services.Interfaces;
using BO.Models;
using BO.Models.Configs;
using CodeReviewApp.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{


    public sealed class ReviewAppEmailService : IReviewAppEmailService
    {
        public static string ConfigurationKey = "CodeReviewApp";

        public const string Subject = "Code Review";

        private readonly IEmailServiceSender _emailService;
        private readonly MailSendingConfig _config;

        private List<OneMail> _forSend;//todo думаю нужен репозиторий и бд
        private readonly object _lock;

        public ReviewAppEmailService(IEmailServiceSender emailService, MailSendingConfig config)
        //IConfiguration configuration)
        {
            _emailService = emailService;
            _config = config;
            _forSend = new List<OneMail>();
            _lock = new object();
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }
            var reviewConfig = _config.Values[ConfigurationKey];
            await _emailService.SendEmailAsync(email, subject, message, reviewConfig);
        }

        public async Task QueueEmailAsync(string email, string subject, string message)
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

        public async Task QueueEmailAsync(List<string> email, string subject, string message)
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

        public async Task SendQueueAsync()
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

            var reviewConfig = _config.Values[ConfigurationKey];
            await _emailService.SendEmailAsync(combinedForSend, reviewConfig);
        }



        //------------------

        public async Task QueueNewCommentInReviewTaskAsync(string email, string taskName)
        {
            await QueueEmailAsync(email, Subject, $"Добавлен новый комментарий в задачу {taskName}");
        }

        public async Task QueueNewCommentInReviewTaskAsync(List<string> email, string taskName)
        {
            await QueueEmailAsync(email, Subject, $"Добавлен новый комментарий в задачу {taskName}");
        }

        public async Task QueueReviewerInReviewTaskAsync(string email, string taskName)
        {
            await QueueEmailAsync(email, Subject, $"Назначение ревьювером по задаче {taskName}");

        }
    }
}
