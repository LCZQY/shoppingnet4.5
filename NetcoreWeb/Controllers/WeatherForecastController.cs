using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetcoreWeb.Models;
namespace NetcoreWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly ShoppingDbContext _dbContext;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ShoppingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }



        /// <summary>
        /// post
        /// </summary>
        /// <returns></returns>
        [HttpPost("post")]
        public JsonResult Post()
        {
           var uses = _dbContext.Users.ToList();
            return new JsonResult(uses);
        }


        /// <summary>
        /// get请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
