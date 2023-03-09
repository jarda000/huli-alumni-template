using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using WeatherApp.Contexts;
using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;
using WeatherAppTests.Helpers;

namespace WeatherAppTests.IntegrationTests
{
    public class UserControllerTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly CustomWebAppFactory _customWebAppFactory;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _client;
        private readonly JwtHelper _jwtHelper;

        public UserControllerTests(CustomWebAppFactory customWebAppFactory)
        {
            _customWebAppFactory = customWebAppFactory;
            _dbContext = TestContextProvider.CreateContextFromFactory(_customWebAppFactory);
            var configuration = new ConfigurationBuilder().AddUserSecrets<UserControllerTests>().Build();
            _jwtHelper = new JwtHelper(configuration);
            _client = _customWebAppFactory.CreateClient();
            var token = _jwtHelper.GenerateJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string password = BCrypt.Net.BCrypt.HashPassword("passworD-01");
            _dbContext.Users.Add(new User { Name = "John Doe", Email = "jarda000@seznam.cz", Password = password, IsEmailConfirmed = true });
            _dbContext.SaveChanges();
        }

        [Theory]
        [InlineData("register", "Jaroslav", "jarda000@seznam.cz", "pass", "Invalid password!")]
        [InlineData("register", "Jaroslav", "jarda000@seznam", "pass", "Invalid email!")]
        [InlineData("register", "156f1sd", "jarda000@seznam.cz", "pass", "Invalid name!")]
        [InlineData("register", "Jaroslav", "jarda000@seznam.cz", "Password-01", "Existing account!")]
        public async Task RegisterErrorResponse(string url, string name, string email, string password, string message)
        {
            //arrange
            var registerDTO = new RegisterDTO { Name = name, Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(registerDTO), Encoding.UTF8, "application/json");

            //act
            var response = await _client.PostAsync(url, content);

            //assert
            Assert.Equal(message, response.Content.ReadAsStringAsync().Result);
        }

        [Theory]
        [InlineData("login", "jarda000@seznam.cz", "pass", "Invalid username or password!")]
        [InlineData("login", "jarda000@seznam", "pass", "Invalid username or password!")]
        public async Task LoginErrorResponse(string url, string email, string password, string message)
        {
            //arrange
            var loginDTO = new LoginDTO { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");

            //act
            var response = await _client.PostAsync(url, content);

            //assert
            Assert.Equal(message, response.Content.ReadAsStringAsync().Result);
        }

        [Theory]
        [InlineData("login", "jarda000@seznam.cz", "passworD-01")]
        public async Task LoginSuccesStatusCode(string url, string email, string password)
        {
            //arrange
            var loginDTO = new LoginDTO { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");

            //act
            var response = await _client.PostAsync(url, content);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
