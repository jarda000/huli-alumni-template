using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IDistributedCache _distributedCache;

        public WeatherService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<WeatherData> FetchWeatherData(string city, string apiKey)
        {
            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

            // Check if weather data for the city is available in the cache
            var cacheData = await _distributedCache.GetStringAsync(city);
            if (!string.IsNullOrEmpty(cacheData))
            {
                return JsonConvert.DeserializeObject<WeatherData>(cacheData);
            }

            // Fetch the weather data from the external API
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(data);

                    // Save the weather data in the cache for a certain amount of time (e.g. 5 minutes)
                    await _distributedCache.SetStringAsync(city, data, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });

                    return weatherData;
                }
                else
                {
                    throw new Exception("Failed to fetch weather data from API");
                }
            }
        }
    }
}
