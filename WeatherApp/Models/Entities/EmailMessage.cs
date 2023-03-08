namespace WeatherApp.Models.Entities
{
    public class EmailMessage
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string HtmlContent { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
