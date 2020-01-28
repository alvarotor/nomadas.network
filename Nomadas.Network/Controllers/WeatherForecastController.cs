﻿using System;
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
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastCore _weatherCore;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastCore weatherCore)
        {
            _weatherCore = weatherCore;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<WeatherForecast> Get()
        {
            _logger.LogInformation("Getting all list");

            var items = _weatherCore.FindAll();

            if (!items.Any())
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpGet("ordered")]
        public ActionResult<WeatherForecast> GetOrdered()
        {
            _logger.LogInformation("Getting ordered list");

            var items = _weatherCore.GetAllOrderBySummary();

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecast>> Create([FromBody]WeatherForecast item)
        {
            try
            {
                if (item == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _logger.LogInformation("Creating new item...");

                await _weatherCore.Create(item);
                await _weatherCore.Save();

                _logger.LogInformation($"Created new item {item.Id}");

                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<WeatherForecast> GetItem(long id)
        {
            _logger.LogInformation($"Getting item {id}");

            var item = _weatherCore.FindByCondition(x => x.Id == id);

            if (item.Any())
            {
                return Ok(item.First());
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<WeatherForecast>> DeleteItem(long id)
        {
            _logger.LogInformation("Deleting item...");

            var item = _weatherCore.FindByCondition(x => x.Id == id);

            if (!item.Any())
            {
                return NotFound();
            }

            _weatherCore.Delete(item.First());
            await _weatherCore.Save();

            _logger.LogInformation($"Deleted item {id}");

            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WeatherForecast>> UpdateItem(long id, [FromBody]WeatherForecast item)
        {
            try
            {
                if (item == null)
                {
                    _logger.LogError("item object sent from client is null.");
                    return BadRequest("item object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid item object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _logger.LogInformation("Updating item...");

                var itemFound = _weatherCore.FindByCondition(x => x.Id == id).First();

                if (itemFound == null)
                {
                    _logger.LogError($"Item with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                itemFound.Date = item.Date;
                itemFound.Summary = item.Summary;
                itemFound.TemperatureC = item.TemperatureC;

                _weatherCore.Update(itemFound);
                await _weatherCore.Save();

                _logger.LogInformation($"Updated item {id}");

                return Ok(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Update action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
