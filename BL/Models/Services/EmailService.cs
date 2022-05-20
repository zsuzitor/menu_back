
using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class EmailService : IEmailServiceSender
    {
        //https://github.com/myloveCc/NETCore.MailKit
        public async Task SendEmailAsync(string nameFrom, string emailFrom
            , string email, string subject, string message,
            string mailingHost, int mailingPort, string mailingLogin, string mailingPassword)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(nameFrom, emailFrom));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailingHost, mailingPort, false);
                await client.AuthenticateAsync(mailingLogin, mailingPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmailAsync(string email, string subject, string message,
            MailSendingInstanceConfig config)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(config.NameFrom, config.EmailFrom));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(config.MailingHost, config.MailingPort, false);
                await client.AuthenticateAsync(config.MailingLogin, config.MailingPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
