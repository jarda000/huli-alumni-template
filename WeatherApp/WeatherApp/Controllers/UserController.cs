using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.DTOs;

namespace WeatherApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : MasterController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserController(IUserService registerService, ITokenService tokenService,IEmailService emailService, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext) : base(httpContextAccessor, dbContext)
        {
            _userService = registerService;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDTO request)
        {
            if(!_userService.ValidName(request.Name))
            {
                return BadRequest("Invalid name!");
            }
            if(!_userService.ValidEmail(request.Email))
            {
                return BadRequest("Invalid email!");
            }
            if(!_userService.ValidPassword(request.Password))
            {
                return BadRequest("Invalid password!");
            }
            if(_userService.EmailExists(request.Email))
            {
                return BadRequest("Existing account!");
            }
            _userService.Register(request);
            return RedirectToAction("Login");
        }

        [HttpPost("login")]
        public IActionResult Login(UserDTO request)
        {
            var user = _userService.GetUser(request);
            if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Invalid username or password!");
            }
            return Ok(_tokenService.CreateToken(user));
        }

        [HttpPost("verify")]
        public IActionResult Verify(string link)
        {
            return Ok();
        }
    }
}
