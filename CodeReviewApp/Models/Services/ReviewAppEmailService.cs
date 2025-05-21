using BL.Models.Services;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using CodeReviewApp.Models.Services.Interfaces;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{


    public sealed class ReviewAppEmailService : EmailServiceBase, IReviewAppEmailService
    {
        private readonly IConfigurationService _configurationService;

        public const string ConfigurationKey = "DefaultMailSettings";
        public const string DefaultSubject = "Code Review";
        private readonly MailSendingInstanceConfig __config;
        protected override MailSendingInstanceConfig _config => __config;
        protected override string Group => "ReviewApp";

        public ReviewAppEmailService(
            IEmailServiceSender emailService, INotificationRepository notificationRepository,
            IConfigurationService configurationService, MailSendingConfig config)
            : base(emailService, notificationRepository)
        {
            __config = config.Values[ConfigurationKey];
            _configurationService = configurationService;
        }


        public async Task QueueNewCommentInReviewTaskAsync(string email, string taskName)
        {

            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.AddedNewCommentInTask);
            var text = config.Value.Replace("{{taskName}}", taskName);
            await QueueEmailAsync(email, DefaultSubject, text);
        }

        public async Task QueueNewCommentInReviewTaskAsync(List<string> email, string taskName)
        {
            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.AddedNewCommentInTask);
            var text = config.Value.Replace("{{taskName}}", taskName);
            await QueueEmailAsync(email, DefaultSubject, text);
        }

        public async Task QueueReviewerInReviewTaskAsync(string email, string taskName)
        {
            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.NewReviewerInTask);
            var text = config.Value.Replace("{{taskName}}", taskName);
            await QueueEmailAsync(email, DefaultSubject, text);
        }

        public async Task QueueChangeStatusTaskAsync(string email, string taskName, string newStatus)
        {
            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.StatusInTaskWasChanged);
            var text = config.Value.Replace("{{taskName}}", taskName).Replace("{{newStatus}}", newStatus);
            await QueueEmailAsync(email, DefaultSubject, text);
        }
    }
}
