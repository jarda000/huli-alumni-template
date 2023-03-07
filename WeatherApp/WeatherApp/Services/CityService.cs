using System.Text.RegularExpressions;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
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

        public void AddCityToMyList(int id, string input)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            var city = new City { Id = id, Name = input };
            user.Cities.Add(city);
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public bool ValidCity(string input)
        {
            return Regex.IsMatch(input, "([a-zA-Z\\s]+)(,\\s[a-zA-Z]{2})?$");
        }
    }
}
