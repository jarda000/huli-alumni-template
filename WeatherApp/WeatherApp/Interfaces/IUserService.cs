using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Interfaces
{
    public interface IUserService
    {
        void Register(UserDTO request);
        User GetUser(string email);
        bool ValidateUser(string email, string token);
        bool EmailExists(string email);
        bool ValidEmail(string email);
        bool ValidPassword(string password);
        bool ValidName(string name);
    }
}