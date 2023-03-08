using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;

namespace WeatherApp.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddCityToMyList(User user, string input)
        {
            _context.Cities.Add(new City { Name = input, User = user });
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public List<CityDTO> GetAll(User user)
        {
            return _context.Cities.Where(c => c.User == user).Select(c => new CityDTO { Name = c.Name }).ToList();
        }

        public CityDTO Get(User user, int id)
        {
            return new CityDTO
            {
                Name = _context.Cities.FirstOrDefault(c => c.User == user && c.Id == id).Name
            };
        }

        public void DeleteCityFromMyList(User user, int id)
        {
            var city = _context.Cities.Include(c => c.User == user).FirstOrDefault(c => c.Id == id);
            _context.Cities.Remove(city);
            _context.SaveChanges();
        }
        public bool ValidCityName(string input)
        {
            return Regex.IsMatch(input, "([a-zA-Z\\s]+)(,\\s[a-zA-Z]{2})?$");
        }

        public bool ValidCity(User user, int id)
        {
            return _context.Cities.Include(x => x.User == user).FirstOrDefault(x => x.Id == id) != null;
        }
    }
}
