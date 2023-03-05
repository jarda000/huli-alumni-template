using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Interfaces
{
    public interface IUserService
    {
        void Register(UserDTO request);
        User GetUser(UserDTO request);
        bool EmailExists(string email);
        bool ValidEmail(string email);
        bool ValidPassword(string password);
    }
}