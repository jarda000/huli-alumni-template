using WeatherApp.Interfaces;

namespace WeatherApp.Services
{
    public class CityService 
    {
        private readonly IDbCityService _dbCityService;

        public CityService(IDbCityService dbCityService)
        {
            _dbCityService = dbCityService;
        }

    }
}
