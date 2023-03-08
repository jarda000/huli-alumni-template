using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;

namespace WeatherApp.Controllers
{
    [Route("/user")]
    [ApiController]
    [Authorize]
    public class UserUpdateController : MasterController
    {
        private readonly IUserService _userService;
        public UserUpdateController(IUserService userService, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context) : base(httpContextAccessor, context)
        {
            _userService = userService;
        }

        [HttpPost("/update-password")]
        public IActionResult UpdatePassword(string password)
        {
            if (!_user.IsEmailConfirmed)
            {
                return BadRequest("Please verify your email!");
            }
            if (!_userService.ValidPassword(password))
            {
                return BadRequest("Invalid password!");
            }
            _userService.UpdatePassword(_user, password);
            return Ok("Your password has been updated");
        }

        [HttpPost("/update-email")]
        public IActionResult UpdateEmail(string email)
        {
            if (!_user.IsEmailConfirmed)
            {
                return BadRequest("Please verify your email!");
            }
            if (!_userService.ValidEmail(email))
            {
                return BadRequest("Invalid email!");
            }
            _userService.UpdateEmail(_user, email);
            return Ok("Your email has been updated");
        }

        [HttpPost("/update-name")]
        public IActionResult UpdateName(string name)
        {
            if (!_user.IsEmailConfirmed)
            {
                return BadRequest("Please verify your email!");
            }
            if (!_userService.ValidName(name))
            {
                return BadRequest("Invalid name!");
            }
            _userService.UpdateName(_user, name);
            return Ok("Your name has been updated");
        }
    }
}
