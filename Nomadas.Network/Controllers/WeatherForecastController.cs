using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly DBContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DBContext context)
        {
            _context = context;
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
        public async Task<ActionResult<WeatherForecast>> PostDBItem(WeatherForecast dbItem)
        {
            _context.DBItems.Add(dbItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDBItem), new { id = dbItem.Id }, dbItem);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecast>> GetDBItem(long id)
        {
            var todoItem = await _context.DBItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
    }
}
