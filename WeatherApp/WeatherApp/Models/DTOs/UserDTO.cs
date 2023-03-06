namespace WeatherApp.Models.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
