using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.Models.Entities
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(1);
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
