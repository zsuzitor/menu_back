using BL.Models.Services;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;

namespace Auth.Models.Auth.Services
{
    public sealed class AuthEmailService : EmailServiceBase
    {
        public const string ConfigurationKey = "DefaultMailSettings";

        private readonly MailSendingInstanceConfig __config;
        protected override MailSendingInstanceConfig _config => __config;


        public AuthEmailService(
            IEmailServiceSender emailService, MailSendingConfig config)
            : base(emailService)
        {
            __config = config.Values[ConfigurationKey];
        }
    }
}
