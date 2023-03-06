using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.DTOs;

namespace WeatherApp.Controllers
{
    [Route("user/")]
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
        public IActionResult Register(RegisterDTO request)
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
            var user = _userService.GetUser(request.Email);
            _emailService.EmailVerification(user);
            return Ok("Registration succesful!");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO request)
        {
            var user = _userService.GetUser(request.Email);
            if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Invalid username or password!");
            }
            return Ok(_tokenService.CreateToken(user));
        }

        [HttpGet("verify")]
        public IActionResult Verify(string email, string token)
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

            email = HttpUtility.UrlEncode(email);
            token = HttpUtility.UrlEncode(token);

            if (!_userService.ValidateUser(email, token))
            {
                return BadRequest("The verification link is invalid or has expired. Please try again.");
            }
            return Ok("Your email address has been verified. Thank you!");
        }

        [HttpPost("password-reset")]
        public IActionResult PasswordReset(string email)
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

        [HttpGet("password-reset")]
        public IActionResult PasswordReset(string email, string token)
        {
            return Ok();
        }

        [HttpPost("{id}/update-password"), Authorize]
        public IActionResult UpdatePassword(string email, string password)
        {
            if(!_userService.ValidPassword(password))
            {
                return BadRequest("Invalid password!");
            }
            _userService.UpdatePassword(email, password);
            return Ok("Your password has been updated");
        }
    }
}
