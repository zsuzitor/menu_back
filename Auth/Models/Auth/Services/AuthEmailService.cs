using BL.Models.Services;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using DAL.Models.DAL.Repositories.Interfaces;

namespace Auth.Models.Auth.Services
{
    public sealed class AuthEmailService : EmailServiceBase
    {
        public const string ConfigurationKey = "DefaultMailSettings";

        private readonly MailSendingInstanceConfig __config;
        protected override MailSendingInstanceConfig _config => __config;
        protected override string Group => "AuthEmail";


        public AuthEmailService(
            IEmailServiceSender emailService, INotificationRepository notificationRepository, MailSendingConfig config, IDateTimeProvider dateTimeProvider)
            : base(emailService, notificationRepository, dateTimeProvider)
        {
            __config = config.Values[ConfigurationKey];
        }
    }
}
