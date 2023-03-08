using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Interfaces
{
    public interface ICityService
    {
        void AddCityToMyList(User user, string input);
        List<CityDTO> GetAll(User user);
        CityDTO Get(User user, int id);
        void DeleteCityFromMyList(User user, int id);
        bool ValidCityName(string input);
        bool ValidCity(User user, int id);
    }
}