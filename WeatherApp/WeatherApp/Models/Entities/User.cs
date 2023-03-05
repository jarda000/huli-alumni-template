namespace WeatherApp.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<User> Cities { get; set; }
    }
}
