using BL.Models.Services;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using CodeReviewApp.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{


    public sealed class ReviewAppEmailService : EmailServiceBase, IReviewAppEmailService
    {
        public override string ConfigurationKey => "DefaultMailSettings";
        public string DefaultSubject => "Code Review";
        private readonly MailSendingConfig __config;
        protected override MailSendingConfig _config => __config;


        public ReviewAppEmailService(
            IEmailServiceSender emailService, MailSendingConfig config)
            :base(emailService)
        {
            __config = config;
        }



        public async Task QueueNewCommentInReviewTaskAsync(string email, string taskName)
        {
            await QueueEmailAsync(email, DefaultSubject, $"Добавлен новый комментарий в задачу {taskName}");
        }

        public async Task QueueNewCommentInReviewTaskAsync(List<string> email, string taskName)
        {
            await QueueEmailAsync(email, DefaultSubject, $"Добавлен новый комментарий в задачу {taskName}");
        }

        public async Task QueueReviewerInReviewTaskAsync(string email, string taskName)
        {
            await QueueEmailAsync(email, DefaultSubject, $"Назначение ревьювером по задаче {taskName}");
        }

        public async Task QueueChangeStatusTaskAsync(string email, string taskName, string newStatus)
        {
            await QueueEmailAsync(email, DefaultSubject, $"Изменен статус на {newStatus} в задаче {taskName}");
        }
    }
}
