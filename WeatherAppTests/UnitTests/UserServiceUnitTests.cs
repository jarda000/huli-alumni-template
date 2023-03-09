using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Contexts;
using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;
using WeatherApp.Services;

namespace WeatherAppTests.UnitTests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Register_AddsUserToContextAndSavesChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Register_AddsUserToContextAndSavesChanges")
                .Options;

            using var context = new ApplicationDbContext(options);
            var userService = new UserService(context);
            var request = new RegisterDTO
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                Password = "P@ssw0rd"
            };

            // Act
            userService.Register(request);

            // Assert
            Assert.Single(context.Users);
            Assert.Equal(request.Name, context.Users.Single().Name);
            Assert.Equal(request.Email, context.Users.Single().Email);
            Assert.False(context.Users.Single().IsEmailConfirmed);
            Assert.True(BCrypt.Net.BCrypt.Verify(request.Password, context.Users.Single().Password));
        }

        [Fact]
        public async Task ValidateUser_ReturnsTrueAndUpdatesUserIfTokenIsValidAndNotExpired()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_ValidateUser_ReturnsTrueAndUpdatesUserIfTokenIsValidAndNotExpired")
                .Options;

            using var context = new ApplicationDbContext(options);
            var userService = new UserService(context);
            var emailVerification = new EmailVerification
            {
                Token = "abc123",
                ExpiryDate = DateTime.Now.AddDays(1),
                CreatedDate = DateTime.Now,
                Email = "johndoe@example.com",
                User = new User
                {
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    IsEmailConfirmed = false,
                    Password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd")
                }
            };

            context.EmailVerifications.Add(emailVerification);
            context.SaveChanges();

            // Act
            var result = userService.ValidateUser(emailVerification.User.Email, emailVerification.Token);

            // Assert
            Assert.True(result);
            Assert.True(context.Users.Single().IsEmailConfirmed);
        }

        [Fact]
        public async Task ValidateUser_ReturnsFalseIfTokenIsExpired()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_ValidateUser_ReturnsFalseIfTokenIsExpired")
                .Options;

            using var context = new ApplicationDbContext(options);
            var userService = new UserService(context);
            var emailVerification = new EmailVerification
            {
                Token = "abc123",
                ExpiryDate = DateTime.Now.AddDays(-1),
                CreatedDate = DateTime.Now,
                Email = "johndoe@example.com",
                User = new User
                {
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    IsEmailConfirmed = false,
                    Password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd")
                }
            };

            context.EmailVerifications.Add(emailVerification);
            context.SaveChanges();

            // Act
            var result = userService.ValidateUser(emailVerification.User.Email, emailVerification.Token);

            // Assert
            Assert.False(result);
            Assert.False(context.Users.Single().IsEmailConfirmed);
        }
    }
}
