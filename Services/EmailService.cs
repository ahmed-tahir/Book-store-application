using BookStoreApplication.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApplication.Services
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"EmailTemplates/{0}.html";
        private readonly SMTPModel _config;

        public EmailService(IOptions<SMTPModel> config)
        {
            _config = config.Value;
        }

        public async Task SendTestEmail(UserEmailOptions emailOptions)
        {
            emailOptions.Subject = UpdatePlaceHolders("Test subject from Bookstore app", emailOptions.PlaceHolders);
            emailOptions.Body = UpdatePlaceHolders(GetEmailBody("MailTemplate"), emailOptions.PlaceHolders);
            await SendEmailAsync(emailOptions);
        }

        public async Task SendEmailConfirmation(UserEmailOptions emailOptions)
        {
            emailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, Confirm your email id", emailOptions.PlaceHolders);
            emailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirmationTemplate"), emailOptions.PlaceHolders);
            await SendEmailAsync(emailOptions);
        }

        private async Task SendEmailAsync(UserEmailOptions emailOptions)
        {
            // Setting up the email fields
            MailMessage mail = new MailMessage()
            {
                Subject = emailOptions.Subject,
                Body = emailOptions.Body,
                From = new MailAddress(_config.SenderAddress, _config.SenderDisplayName),
                IsBodyHtml = _config.IsBodyHTML,
                BodyEncoding = Encoding.Default
            };

            // extracting email addresses to send mail to
            foreach (var toEmail in emailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential credentials = new NetworkCredential(_config.UserName, _config.Password);

            // Configuring the SMTP client object
            SmtpClient client = new SmtpClient
            {
                Host = _config.Host,
                Port = _config.Port,
                EnableSsl = _config.EnableSSL,
                UseDefaultCredentials = _config.UseDefaultCredentials,
                Credentials = credentials
            };

            // Sending the mail through SMTP protocol
            await client.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;

        }

        private string  UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if(!String.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                keyValuePairs.ForEach(kvp => {
                    if (text.Contains(kvp.Key))
                        text = text.Replace(kvp.Key, kvp.Value);
                });
            }
            return text;
        }
    }
}
