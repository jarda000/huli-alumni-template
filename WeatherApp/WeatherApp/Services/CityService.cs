using WeatherApp.Contexts;
using WeatherApp.Interfaces;

namespace WeatherApp.Services
{
    public class CityService 
    {
        private readonly ApplicationDbContext _context;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
