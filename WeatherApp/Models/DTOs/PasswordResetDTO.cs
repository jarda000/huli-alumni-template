namespace WeatherApp.Models.DTOs
{
    public class PasswordResetDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
