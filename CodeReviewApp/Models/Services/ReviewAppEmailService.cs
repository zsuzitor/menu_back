﻿using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public class ReviewAppEmailService : IEmailService
    {
        public static string ConfigurationKey = "CodeReviewApp";

        private readonly IEmailServiceSender _emailService;
        private readonly MailSendingConfig _config;

        public ReviewAppEmailService(IEmailServiceSender emailService, MailSendingConfig config)
        //IConfiguration configuration)
        {
            _emailService = emailService;
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var reviewConfig = _config.Values[ConfigurationKey];
            await _emailService.SendEmailAsync(reviewConfig.NameFrom, reviewConfig.EmailFrom
                , email, subject, message, reviewConfig.MailingHost
                , reviewConfig.MailingPort, reviewConfig.MailingLogin
                , reviewConfig.MailingPassword);
        }
    }
}
