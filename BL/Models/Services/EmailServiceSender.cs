
using BL.Models.Services.Interfaces;
using BO.Models;
using BO.Models.Configs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class EmailServiceSender : IEmailServiceSender
    {
        private readonly ILogger _logger;
        public EmailServiceSender(ILogger<EmailServiceSender> logger)
        {
            _logger = logger;
        }

        //https://github.com/myloveCc/NETCore.MailKit
        public async Task SendEmailAsync(string nameFrom, string emailFrom
            , string email, string subject, string message,
            string mailingHost, int mailingPort, string mailingLogin, string mailingPassword)
        {
            var conf = new MailSendingInstanceConfig()
            {
                EmailFrom = emailFrom,
                MailingHost = mailingHost,
                MailingPort = mailingPort,
                MailingLogin = mailingLogin,
                MailingPassword = mailingPassword,
                NameFrom = nameFrom,
            };
            await SendEmailAsync(email, subject, message, conf);
        }

        public async Task SendEmailAsync(string email, string subject, string message,
            MailSendingInstanceConfig config)
        {
            await SendEmailAsync(
                new OneMail() { Email = email, Message = message, Subject = subject }, config);
        }

        public async Task SendEmailAsync(OneMail oneMail, MailSendingInstanceConfig config)
        {
            await SendEmailAsync(new List<OneMail>() { oneMail }, config);
        }

        public virtual async Task SendEmailAsync(List<OneMail> mails, MailSendingInstanceConfig config)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    try
                    {

                        await client.ConnectAsync(config.MailingHost, config.MailingPort, false);
                        await client.AuthenticateAsync(config.MailingLogin, config.MailingPassword);
                        foreach (var mail in mails)
                        {
                            var emailMessage = new MimeMessage();

                            emailMessage.From.Add(new MailboxAddress(config.NameFrom, config.EmailFrom));
                            emailMessage.To.Add(new MailboxAddress("", mail.Email));
                            emailMessage.Subject = mail.Subject;
                            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                            {
                                Text = mail.Message
                            };
                            await client.SendAsync(emailMessage);

                        }
                    }
                    catch
                    {
                        await client.DisconnectAsync(true);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ошибка отправки email");
                //todo хотя бы логи записать
            }
        }
    }

    public class EmailServiceSenderMock : EmailServiceSender
    {
        public EmailServiceSenderMock(ILogger<EmailServiceSender> logger) : base(logger)
        {
            
        }

        public override async Task SendEmailAsync(List<OneMail> mails, MailSendingInstanceConfig config)
        {
            var resultFilePath = "./MailMock.txt";
            //if (!File.Exists(resultFilePath))
            //{
            //    File.(resultFilePath);
            //}

            File.AppendAllLines(resultFilePath, mails.Select(x =>
            $@"{DateTime.Now}-from:{config.EmailFrom}-to:{x.Email}-subject:{x.Subject}-message:{x.Message}"));
        }
    }
}
