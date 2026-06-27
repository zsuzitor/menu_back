
using BL.Models.Services.Interfaces;
using BO.Models;
using BO.Models.Configs;
using Common.Models;
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
        protected readonly IDateTimeProvider _dateTimeProvider;
        public EmailServiceSender(ILoggerFactory loggerFactory, IDateTimeProvider dateTimeProvider)
        {
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);
            _dateTimeProvider = dateTimeProvider;

        }

        //https://github.com/myloveCc/NETCore.MailKit
        public async Task<bool> SendEmailAsync(string nameFrom, string emailFrom
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
            return await SendEmailAsync(email, subject, message, conf);
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message,
            MailSendingInstanceConfig config)
        {
            return await SendEmailAsync(
                new OneMail() { Email = email, Message = message, Subject = subject }, config);
        }

        public async Task<bool> SendEmailAsync(OneMail oneMail, MailSendingInstanceConfig config)
        {
            var res = await SendEmailAsync(new List<OneMail>() { oneMail }, config);
            return res.Count == 0;
        }

        public virtual async Task<List<long>> SendEmailAsync(List<OneMail> mails, MailSendingInstanceConfig config)
        {
            var errors = new List<long>();
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
                            try
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
                            catch (Exception e)
                            {
                                _logger.LogError(e, "ошибка отправки email id={Id}", mail.Id);
                                errors.Add(mail.Id);
                            }

                        }
                        await client.DisconnectAsync(true);
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
                throw;
            }

            return errors;
        }
    }

    public class EmailServiceSenderMock : EmailServiceSender
    {
        public EmailServiceSenderMock(ILoggerFactory loggerFactory, IDateTimeProvider dateTimeProvider) : base(loggerFactory, dateTimeProvider)
        {
        }

        public override async Task<List<long>> SendEmailAsync(List<OneMail> mails, MailSendingInstanceConfig config)
        {
            var resultFilePath = "./MailMock.txt";
            //if (!File.Exists(resultFilePath))
            //{
            //    File.(resultFilePath);
            //}

            File.AppendAllLines(resultFilePath, mails.Select(x =>
            $@"{_dateTimeProvider.CurrentDateTime()}-from:{config.EmailFrom}-to:{x.Email}-subject:{x.Subject}-message:{x.Message}"));
            return [];
        }
    }
}
