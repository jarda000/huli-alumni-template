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

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Register(RegisterDTO request)
        {
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool ValidateUser(string email, string token)
        {
            var user = GetUser(email);
            if (user.EmailVerification.Token == token)
            {
                user.IsEmailConfirmed = true;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void UpdatePassword(string email, string password)
        {
            var user = GetUser(email);
            user.Password = password;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetUser(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email) != null;
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
