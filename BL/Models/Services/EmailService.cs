
using BL.Models.Services.Interfaces;
using BO.Models;
using BO.Models.Configs;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public sealed class EmailService : IEmailServiceSender
    {
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
            await SendEmailAsync(new OneMail()
            { Email = email, Message = message, Subject = message }, config);
        }

        public async Task SendEmailAsync(OneMail oneMail, MailSendingInstanceConfig config)
        {
            await SendEmailAsync(new List<OneMail>() { oneMail }, config);
        }

        public async Task SendEmailAsync(List<OneMail> mails, MailSendingInstanceConfig config)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(config.MailingHost, config.MailingPort, false);
                    try
                    {
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
            catch
            {
                //todo хотя бы логи записать
            }
        }
    }
}
