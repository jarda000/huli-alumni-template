using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.Entities;

namespace WeatherApp.Controllers
{
    [Route("/user")]
    [ApiController]
    [Authorize]
    public class UserUpdateController : MasterController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserUpdateController> _logger;
        public UserUpdateController(IUserService userService, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, ILogger<UserUpdateController> logger) : base(httpContextAccessor, context)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("/update-password")]
        public IActionResult UpdatePassword(string password)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during password update");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during password update. Please try again later.");
            }
        }

        [HttpPost("/update-email")]
        public IActionResult UpdateEmail(string email)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured during email update");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during email update. Please try again later.");
            }
        }

        [HttpPost("/update-name")]
        public IActionResult UpdateName(string name)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during name update");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during name update. Please try again later.");
            }
        }
    }
}
