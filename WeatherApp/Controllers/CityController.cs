using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    [Route("/city")]
    [ApiController]
    [Authorize]
    public class CityController : MasterController
    {
        private readonly ICityService _cityService;
        private readonly IWeatherService _weatherService;
        private readonly IConfiguration _configuration;
        public CityController(IHttpContextAccessor httpContextAccessor, ICityService cityService, IWeatherService weatherService, IConfiguration configuration, ApplicationDbContext context) : base(httpContextAccessor, context)
        {
            _cityService = cityService;
            _weatherService = weatherService;
            _configuration = configuration;
        }

        [HttpPost("/add")]
        public IActionResult AddCityToMyList(string city)
        {
            if(!_cityService.ValidCityName(city))
            {
                return BadRequest("Invalid city name");
            }
            _cityService.AddCityToMyList(_user, city);
            return Ok($"{city} added to your list");
        }

        [HttpPost("/remove/{id}")]
        public IActionResult RemoveCityFromMyList(int id)
        {
            if(!_cityService.ValidCity(_user, id))
            {
                return BadRequest("Invalid city Id");
            }
            _cityService.DeleteCityFromMyList(_user, id);
            return Ok("City removed from the list");
        }

        [HttpGet("/my-city-list")]
        public IActionResult GetMyCities()
        {
            return Ok(_cityService.GetAll(_user));
        }

        [HttpGet("/weather")]
        public async Task<ActionResult<WeatherData>> GetWeatherData(string city)
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
