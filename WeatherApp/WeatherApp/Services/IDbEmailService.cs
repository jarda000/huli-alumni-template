using WeatherApp.Models.Entities;

namespace WeatherApp.Services
{
    public interface IDbEmailService
    {
        void Add(EmailMessage emailMessage);
        void Delete(EmailMessage emailMessage);
        List<EmailMessage> GetAll();
        EmailMessage GetById(int id);
        void Update(EmailMessage emailMessage);
    }
}