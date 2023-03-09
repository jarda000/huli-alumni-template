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

        [HttpPost("resend-verification")]
        public IActionResult ResendVerification(string email)
        {
            try
            {
                if (!_userService.ValidEmail(email))
                {
                    return BadRequest("Invalid email!");
                }
                var user = _userService.GetUser(email);
                if (user == null)
                {
                    return BadRequest("Invalid email!");
                }
                _emailService.EmailVerification(user);
                return Ok("New verification email has been sent to your email address!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during new verification email request");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during request. Please try again later.");
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
        public IActionResult Verify(EmailVerificationDTO emailVerificationDTO)
        {
            try
            {
                if (!_userService.ValidEmail(emailVerificationDTO.Email))
                {
                    return BadRequest("Invalid email address.");
                }
                emailVerificationDTO.Email = emailVerificationDTO.Email.Trim();

                if (string.IsNullOrEmpty(emailVerificationDTO.Token))
                {
                    return BadRequest("Invalid token.");
                }
                emailVerificationDTO.Token = emailVerificationDTO.Token.Trim();

                if (!_userService.ValidateUser(emailVerificationDTO.Email, emailVerificationDTO.Token))
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
        public IActionResult PasswordReset(PasswordResetDTO passwordResetDTO)
        {
            try
            {
                if (!_userService.ValidEmail(passwordResetDTO.Email))
                {
                    return BadRequest("Invalid email address.");
                }
                passwordResetDTO.Email = passwordResetDTO.Email.Trim();

                if (string.IsNullOrEmpty(passwordResetDTO.Token))
                {
                    return BadRequest("Invalid token.");
                }
                passwordResetDTO.Token = passwordResetDTO.Token.Trim();

                if (!_userService.ValidPassword(passwordResetDTO.Password))
                {
                    return BadRequest("Invalid password!");
                }

                if(!_userService.ValidPasswordReset(passwordResetDTO))
                {
                    return BadRequest("Your password reset link is invalid or expired");
                }

                var user = _userService.GetUser(passwordResetDTO.Email);
                _userService.UpdatePassword(user, passwordResetDTO.Password);
                return Ok("Your password reset is done, you can login now");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during passwrod reset");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured during passwrod reset. Please try again later.");
            }

        }
    }
}
