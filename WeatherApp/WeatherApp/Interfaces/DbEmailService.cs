using WeatherApp.Contexts;
using WeatherApp.Models.Entities;
using WeatherApp.Services;

namespace WeatherApp.Interfaces
{
    public class DbEmailService : IDbEmailService
    {
        private readonly ApplicationDbContext _context;

        public DbEmailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(EmailMessage emailMessage)
        {
            _context.EmailMessages.Add(emailMessage);
            _context.SaveChanges();
        }

        public void Update(EmailMessage emailMessage)
        {
            _context.EmailMessages.Update(emailMessage);
            _context.SaveChanges();
        }

        public void Delete(EmailMessage emailMessage)
        {
            _context.EmailMessages.Remove(emailMessage);
            _context.SaveChanges();
        }

        public EmailMessage GetById(int id)
        {
            return _context.EmailMessages.FirstOrDefault(x => x.Id == id);
        }

        public List<EmailMessage> GetAll()
        {
            return _context.EmailMessages.ToList();
        }
    }
}
