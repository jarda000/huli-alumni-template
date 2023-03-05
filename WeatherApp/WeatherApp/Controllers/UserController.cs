using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Interfaces;
using WeatherApp.Models.DTOs;

namespace WeatherApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService registerService)
        {
            _userService = registerService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDTO request)
        {
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

        [HttpGet("login")]
        public IActionResult Login(UserDTO request)
        {
            var user = _userService.GetUser(request);
            if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Invalid username or password!");
            }
            return Ok(_tokenService.CreateToken(user));
        }
    }
}
