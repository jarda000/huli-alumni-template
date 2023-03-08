using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using WeatherApp.Contexts;
using WeatherApp.Interfaces;
using WeatherApp.Models.Entities;

namespace WeatherApp.Controllers
{
    [Route("")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        protected User _user;

        public MasterController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;

            var id = _httpContextAccessor.HttpContext.Items.FirstOrDefault(i => i.Key == "Id").Value;
            _user = _context.Users.FirstOrDefault(u => u.Id.Equals(id));
        }
    }
}
