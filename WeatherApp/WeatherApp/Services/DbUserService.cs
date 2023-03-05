using System.Text.RegularExpressions;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Services
{
    public class DbUserService : IDbUserService
    {
        private readonly ApplicationDbContext _context;

        public DbUserService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User Get(UserDTO request)
        {
            return _context.Users.FirstOrDefault(x => x.Email == request.Email);
        }
    }
}
