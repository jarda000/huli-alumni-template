﻿using SendGrid.Helpers.Mail;
using SendGrid;
using WeatherApp.Contexts;
using WeatherApp.Models.Entities;
using SendGrid.Helpers.Mail.Model;
using WeatherApp.Interfaces;
using System.Web;

namespace WeatherApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public EmailService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task SendEmail(EmailMessage emailMessage)
        {
            var apiKey = _config.GetSection("EmailToken").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("weatherapp@seznam.cz", "WeatherApp");
            var to = new EmailAddress(emailMessage.EmailAddress, emailMessage.User.Name);
            var msg = MailHelper.CreateSingleEmail(from, to, emailMessage.EmailSubject, emailMessage.EmailBody, emailMessage.HtmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public void EmailVerification(User user)
        {
            string token = Guid.NewGuid().ToString();
            string baseUrl = "http://localhost:5110/api/verify";
            string url = $"{baseUrl}?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";

            var emailVerification = new EmailVerification
            {
                Email = HttpUtility.UrlEncode(user.Email),
                Token = HttpUtility.UrlEncode(token),
                User = user,
            };
            _context.EmailVerification.Add(emailVerification);
            _context.SaveChanges();

            var verificationEmail = new EmailMessage
            {
                EmailAddress = user.Email,
                EmailSubject = "WeatherApp verification",
                EmailBody = $"Dear {user.Name},/Please verify you new account, the link is active for 24 hours./Have a nice day, WeatherApp",
                HtmlContent = url,
                User = user,
            };
            _context.EmailMessages.Add(verificationEmail);
            _context.SaveChanges();
            this.SendEmail(verificationEmail);
        }

        public void PasswordReset(string email)
        {
            string token = Guid.NewGuid().ToString();
            string baseUrl = "http://localhost:5110/api/verify";
            string url = $"{baseUrl}?email={HttpUtility.UrlEncode(email)}&token={HttpUtility.UrlEncode(token)}";

            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            var passwordReset = new PasswordReset
            {
                Email = HttpUtility.UrlEncode(email),
                Token = HttpUtility.UrlEncode(token),
                User = user,
            };
            _context.PasswordReset.Add(passwordReset);
            _context.SaveChanges();

            var verificationEmail = new EmailMessage
            {
                EmailAddress = user.Email,
                EmailSubject = "WeatherApp password reset",
                EmailBody = $"Dear {user.Name},/Here is you link for password reset, the link is active for 10 minutes./Have a nice day, WeatherApp",
                HtmlContent = url,
                User = user,
            };
            _context.EmailMessages.Add(verificationEmail);
            _context.SaveChanges();
            this.SendEmail(verificationEmail);
        }
    }
}