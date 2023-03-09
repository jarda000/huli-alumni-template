using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Interfaces
{
    public interface IUserService
    {
        void Register(RegisterDTO request);
        User GetUser(string email);
        bool ValidateUser(string email, string token);
        bool ValidPasswordReset(string email, string token);
        void UpdatePassword(User user, string password);
        void UpdateEmail(User user, string email);
        void UpdateName(User user, string name);
        public User GetById(int id);
        bool EmailExists(string email);
        bool ValidEmail(string email);
        bool ValidPassword(string password);
        bool ValidName(string name);
    }
}