using BL.Models.Services;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using TaskManagementApp.Models.Services.Interfaces;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services
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



        public async Task QueueNewCommentInTaskAsync(List<string> email, string taskName, string taskUrl)
        {
            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.AddedNewCommentInTask);
            var text = config.Value.Replace("{{taskName}}", taskName ?? "")
                .Replace("{{taskUrl}}", taskUrl);
            await QueueEmailAsync(email, DefaultSubject, text);
        }

        public async Task QueueExecutorInTaskAsync(List<string> email, string taskName, string taskUrl)
        {
            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.NewReviewerInTask);
            var text = config.Value.Replace("{{taskName}}", taskName ?? "")
                .Replace("{{taskUrl}}", taskUrl);
            await QueueEmailAsync(email, DefaultSubject, text);
        }

        public async Task QueueChangeStatusTaskAsync(List<string> email, string taskName, string newStatus, string taskUrl)
        {
            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.StatusInTaskWasChanged);
            var text = config.Value.Replace("{{taskName}}", taskName ?? "")
                .Replace("{{newStatus}}", newStatus ?? "")
                .Replace("{{taskUrl}}", taskUrl);
            await QueueEmailAsync(email, DefaultSubject, text);
        }

        public async Task QueueChangeTaskAsync(List<string> email, string taskName, List<Changes> changes, string taskUrl)
        {

            var config = await _configurationService.GetAsync(Consts.EmailConfigurationsCode.TaskWasChanged);
            var str = new StringBuilder();
            foreach (var change in changes)
            {
                str.Append($"{change.PropName}:\nБыло: {change.PropPrevValue??""}\nСтало: {change.PropNewValue ?? ""}\n");
            }

            var text = config.Value.Replace("{{taskName}}", taskName ?? "")
                .Replace("{{changedProp}}", str.ToString())
                .Replace("{{taskUrl}}", taskUrl);

            await QueueEmailAsync(email, DefaultSubject, text);
        }
    }
}
