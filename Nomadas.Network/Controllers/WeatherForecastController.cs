using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomadas.Network.Core;
using Nomadas.Network.Models;

namespace Nomadas.Network.Controllers
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
        // private readonly DBContext _context;
        private readonly IWeatherForecastCore _weatherCore;

        // public WeatherForecastController(ILogger<WeatherForecastController> logger, DBContext context)
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastCore weatherCore)
        {
            // _context = context;
            _weatherCore = weatherCore;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecast>> Post(WeatherForecast dbItem)
        {
            try
            {
                _logger.LogInformation($"Created new item.");

                await _weatherCore.Create(dbItem);
                await _weatherCore.Save();

                return CreatedAtAction(nameof(GetDBItem), new { id = dbItem.Id }, dbItem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<WeatherForecast> GetDBItem(long id)
        {
            var todoItem = _weatherCore.FindAll();

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }
    }
}
