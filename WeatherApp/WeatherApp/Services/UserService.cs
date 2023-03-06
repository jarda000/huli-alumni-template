using BCrypt.Net;
using System.Text.RegularExpressions;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbUserService _dbUserService;

        public UserService(ApplicationDbContext context, IDbUserService dbUserService)
        {
            _context = context;
            _dbUserService = dbUserService;
        }

        public void Register(UserDTO request)
        {
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            _dbUserService.Add(user);
        }

        public User GetUser(UserDTO request)
        {
            return _dbUserService.Get(request);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(p => p.Email == email);
        }

        public bool ValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public bool ValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$");
        }

        public bool ValidName(string name)
        {
            return Regex.IsMatch(name, "^[a-zA-Z]+$");
        }
    }
}
