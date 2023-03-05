using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.Entities;

namespace WeatherApp.Services
{
    public class DbCityService : IDbCityService
    {
        private readonly ApplicationDbContext _context;

        public DbCityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
        }

        public void Update(City city)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();
        }

        public void Delete(City city)
        {
            _context.Cities.Remove(city);
            _context.SaveChanges();
        }

        public City GetById(int id)
        {
            return _context.Cities.FirstOrDefault(x => x.Id == id);
        }

        public List<City> GetAll()
        {
            return _context.Cities.ToList();
        }
    }
}
