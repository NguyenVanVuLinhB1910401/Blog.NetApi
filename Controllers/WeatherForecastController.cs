using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiBlog.Models;

namespace WebApiBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(UserManager<AppUser> userManager, ILogger<WeatherForecastController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // [HttpGet(Name = "GetWeatherForecast")]
        // [Authorize]
        // public async Task<IActionResult> Get()
        // {
        //     var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //     var userId = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
        //     var user = await _userManager.FindByEmailAsync(userId);
        //     return Ok();
        // }


    }
}