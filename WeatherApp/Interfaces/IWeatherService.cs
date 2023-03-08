using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherData> FetchWeatherData(string city, string apiKey);
    }
}