using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    [Route("user")]
    [ApiController]
    [Authorize]
    public class CityController : MasterController
    {
        private readonly ICityService _cityService;
        private readonly IWeatherService _weatherService;
        private readonly IConfiguration _configuration;
        public CityController(IHttpContextAccessor httpContextAccessor, ICityService cityService, IWeatherService weatherService, IConfiguration configuration, ApplicationDbContext applicationDbContext) : base(httpContextAccessor, applicationDbContext)
        {
            _cityService = cityService;
            _weatherService = weatherService;
            _configuration = configuration;
        }

        [HttpPost("{id}/add")]
        public IActionResult AddCityToMyList(int id, string city)
        {
            if(!_cityService.ValidCity(city))
            {
                return BadRequest("");
            }
            _cityService.AddCityToMyList(id, city);
            return Ok(city);
        }

        [HttpGet("weather")]
        public async Task<ActionResult<WeatherData>> GetWeatherData([FromQuery] string city)
        {
            var apiKey = _configuration.GetSection("WeatherAPIKey").Value;
            try
            {
                var weatherData = await _weatherService.FetchWeatherData(city, apiKey);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to fetch weather data: {ex.Message}");
            }
        }
    }
}
