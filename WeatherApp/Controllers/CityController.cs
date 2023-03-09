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
        private readonly ILogger<CityController> _logger;
        public CityController(IHttpContextAccessor httpContextAccessor, ICityService cityService, IWeatherService weatherService, IConfiguration configuration, ApplicationDbContext context, ILogger<CityController> logger) : base(httpContextAccessor, context)
        {
            _cityService = cityService;
            _weatherService = weatherService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("/add")]
        public IActionResult AddCityToMyList(string city)
        {
            try
            {
                if (!_cityService.ValidCityName(city))
                {
                    return BadRequest("Invalid city name");
                }
                _cityService.AddCityToMyList(_user, city);
                return Ok($"{city} added to your list");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during adding city to user list");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please try again later.");
            }
        }

        [HttpPost("/remove/{id}")]
        public IActionResult RemoveCityFromMyList(int id)
        {
            try
            {
                if (!_cityService.ValidCity(_user, id))
                {
                    return BadRequest("Invalid city Id");
                }
                _cityService.DeleteCityFromMyList(_user, id);
                return Ok("City removed from the list");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured during removing city id{id} from user id {_user.Id} list");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please try again later.");
            }
        }

        [HttpGet("/my-city-list")]
        public IActionResult GetMyCities()
        {
            try
            {
                return Ok(_cityService.GetAll(_user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured during getting list of cities for user id {_user.Id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please try again later.");
            }
        }

        [HttpGet("/weather")]
        public async Task<ActionResult<WeatherData>> GetWeatherData(string city)
        {
            var apiKey = _configuration.GetSection("WEATHER_API_KEY").Value ?? Environment.GetEnvironmentVariable("WEATHER_API_KEY");
            try
            {
                var weatherData = await _weatherService.FetchWeatherData(city, apiKey);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting weather data for {city}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. Please try again later.");
            }
        }
    }
}
