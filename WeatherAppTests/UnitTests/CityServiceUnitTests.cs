using Microsoft.EntityFrameworkCore;
using WeatherApp.Contexts;
using WeatherApp.Models.Entities;
using WeatherApp.Services;

namespace WeatherAppTests.UnitTests
{
    public class CityServiceTests
    {
        private ApplicationDbContext _context;
        private CityService _cityService;
        private User user;

        public CityServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            user = new User
            {   Id = 1,
                Name = "John Doe",
                Email = "johndoe@example.com",
                Password = "P@ssw0rd",
                IsEmailConfirmed = true
            };
            _context.Users.Add(user);
            _cityService = new CityService(_context);
        }

        [Theory]
        [InlineData("New York")]
        [InlineData("Los Angeles, CA")]
        public void ValidCityName_Should_ReturnTrue_When_ValidInputIsProvided(string input)
        {
            // Act
            var result = _cityService.ValidCityName(input);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("New York, 123")]
        [InlineData("123")]
        [InlineData("")]
        public void ValidCityName_Should_ReturnFalse_When_InvalidInputIsProvided(string input)
        {
            // Act
            var result = _cityService.ValidCityName(input);

            // Assert
            Assert.False(result);
        }
    }
}
