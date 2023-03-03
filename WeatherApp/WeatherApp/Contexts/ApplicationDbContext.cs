using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
