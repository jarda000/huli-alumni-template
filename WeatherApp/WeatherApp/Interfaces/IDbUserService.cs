using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Interfaces
{
    public interface IDbUserService
    {
        void Add(User user);
        void Delete(User user);
        User Get(UserDTO request);
        void Update(User user);
    }
}