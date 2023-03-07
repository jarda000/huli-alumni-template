using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using WeatherApp.Contexts;
using WeatherApp.Models.Entities;

namespace WeatherApp.Controllers
{
    [Route("")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        protected IHttpContextAccessor _httpContextAccessor;
        protected ApplicationDbContext _applicationDbContext;
        protected User user;

        public MasterController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;

            var userId = _httpContextAccessor.HttpContext.Items.FirstOrDefault(i => i.Key == "Id").Value;
            user = _applicationDbContext.Users.FirstOrDefault(p => p.Id.Equals(userId));
        }
    }
}
