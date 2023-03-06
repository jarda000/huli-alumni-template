using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        [ForeignKey("EmailVerification")]
        public int? EmailVerificationId { get; set; }
        public EmailVerification? EmailVerification { get; set; }
        [ForeignKey("PasswordReset")]
        public int? PasswordResetId { get; set; }
        public PasswordReset? PasswordReset { get; set; }
        public List<EmailMessage> EmailMessages { get; set; }
        public List<City> Cities { get; set; }
    }
}
