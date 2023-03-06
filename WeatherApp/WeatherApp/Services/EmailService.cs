using SendGrid.Helpers.Mail;
using SendGrid;
using WeatherApp.Contexts;
using WeatherApp.Models.Entities;
using SendGrid.Helpers.Mail.Model;
using WeatherApp.Interfaces;
using WeatherApp.Models.DTOs;
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

        public void EmailVerification(UserDTO request)
        {
            string token = Guid.NewGuid().ToString();
            string baseUrl = "http://localhost:5110/api/verify";
            string url = $"{baseUrl}?email={HttpUtility.UrlEncode(request.Email)}&token={HttpUtility.UrlEncode(token)}";

            var verificationEmail = new EmailMessage
            {
                EmailAddress = request.Email,
                EmailSubject = "WeatherApp verification",
                EmailBody = $"Dear {request.Name},/Please verify you new account, the link is active for 24 hours./Have a nice day, WeatherApp",
                HtmlContent = url,
            };
        }
    }
}
