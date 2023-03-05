using WeatherApp.Models.Entities;

namespace WeatherApp.Interfaces
{
    public interface IDbCityService
    {
        void Add(City city);
        void Delete(City city);
        City GetById(int id);
        List<City> GetAll();
        void Update(City city);
    }
}