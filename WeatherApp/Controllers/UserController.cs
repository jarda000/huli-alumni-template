using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Models.DTOs;
using WeatherApp.Models.Entities;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    [Route("")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(ITokenService tokenService, IEmailService emailService, IUserService userService, ILogger<UserController> logger)
        {
            _tokenService = tokenService;
            _emailService = emailService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("/register")]
        public IActionResult Register(RegisterDTO request)
        {
            try
            {
                if (!_userService.ValidName(request.Name))
                {
                    return BadRequest("Invalid name!");
                }
                if (!_userService.ValidEmail(request.Email))
                {
                    return BadRequest("Invalid email!");
                }
                if (!_userService.ValidPassword(request.Password))
                {
                    return BadRequest("Invalid password!");
                }
                if (_userService.EmailExists(request.Email))
                {
                    return BadRequest("Existing account!");
                }
                _userService.Register(request);
                var user = _userService.GetUser(request.Email);
                _emailService.EmailVerification(user);
                return Ok("Registration succesful, please verify your account, email has been sent to your email address!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during registration");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during registration. Please try again later.");
            }

        }

        [HttpPost("/login")]
        public IActionResult Login(LoginDTO request)
        {
            try
            {
                var user = _userService.GetUser(request.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return BadRequest("Invalid username or password!");
                }
                return Ok(_tokenService.CreateToken(user));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured during login");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during login. Please try again later.");
            }
        }

        [HttpGet("/verify")]
        public IActionResult Verify(string email, string token)
        {
            try
            {
                if (!_userService.ValidEmail(email))
                {
                    return BadRequest("Invalid email address.");
                }
                email = email.Trim();

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Invalid token.");
                }
                token = token.Trim();

                if (!_userService.ValidateUser(email, token))
                {
                    return BadRequest("The verification link is invalid or has expired. Please try again.");
                }
                return Ok("Your email address has been verified. Thank you!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured during email verification");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during email verification. Please try again later.");
            }
        }

        [HttpPost("/password-reset")]
        public IActionResult PasswordReset(string email)
        {
            try
            {
                if (!_userService.ValidEmail(email))
                {
                    return BadRequest("Invalid email!");
                }
                if (!_userService.EmailExists(email))
                {
                    return BadRequest("No account registered for this email!");
                }
                _emailService.PasswordReset(email);
                return Ok("Link for password reset has been sent, check your email!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during password reset");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during password reset. Please try again later.");
            }
        }

        [HttpGet("/password-reset")]
        public IActionResult PasswordReset(string email, string token)
        {
            try
            {
                if (!_userService.ValidEmail(email))
                {
                    return BadRequest("Invalid email address.");
                }
                email = email.Trim();

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Invalid token.");
                }
                token = token.Trim();

                var user = _userService.GetUser(email);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during passwrod reset");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during passwrod reset. Please try again later.");
            }

        }
    }
}
