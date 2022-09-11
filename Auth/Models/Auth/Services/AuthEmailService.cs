using BL.Models.Services;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;

namespace Auth.Models.Auth.Services
{
    public sealed class AuthEmailService : EmailServiceBase
    {
        public override string ConfigurationKey => "DefaultMailSettings";

        private readonly MailSendingConfig __config;
        protected override MailSendingConfig _config => __config;


        public AuthEmailService(
            IEmailServiceSender emailService, MailSendingConfig config)
            : base(emailService)
        {
            __config = config;
        }
    }
}
