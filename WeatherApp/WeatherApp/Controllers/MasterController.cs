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

        public MasterController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var id = _httpContextAccessor.HttpContext.Items.FirstOrDefault(i => i.Key == "Id").Value;
        }
    }
}
