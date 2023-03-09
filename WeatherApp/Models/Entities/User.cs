using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public List<EmailVerification> EmailVerifications { get; set; }
        public List<PasswordReset> PasswordResets { get; set; }
        public List<EmailMessage> EmailMessages { get; set; }
        public List<City> Cities { get; set; }
    }
}
