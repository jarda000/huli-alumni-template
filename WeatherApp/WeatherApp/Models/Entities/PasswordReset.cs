namespace WeatherApp.Models.Entities
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddMinutes(10);
        public User User { get; set; }
    }
}
