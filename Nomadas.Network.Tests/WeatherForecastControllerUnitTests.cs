using System.Threading.Tasks;
using Nomadas.Network.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using GenFu;
using System;
using Nomadas.Network.Controllers;
using Nomadas.Network.Core;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Nomadas.Network.Tests
{
    public class WeatherForecastControllerUnitTests
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger;

        public WeatherForecastControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<WeatherForecastController>>();

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var i = 1;
                var items = A.ListOf<WeatherForecast>(25);
                items.ForEach(x =>
                {
                    x.Id = i++;
                    context.DBItems.Add(x);
                });
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task Create()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var core = new WeatherForecastCore(context);
                var controller = new WeatherForecastController(_mockLogger.Object, core);
                var item = A.New<WeatherForecast>();
                item.Summary = "Summary";
                var result = await controller.Create(item) as CreatedAtActionResult;
                Assert.IsType<CreatedAtActionResult>(result);
                var itemResult = result.Value as WeatherForecast;
                Assert.IsType<WeatherForecast>(itemResult);
                // Console.WriteLine(itemResult.Id);
                // Console.WriteLine(itemResult.Summary);
                // Console.WriteLine(itemResult.RandomString);
                // Console.WriteLine(itemResult.TemperatureC);
                Assert.Equal("Summary", itemResult.Summary);
            }
        }

                [Fact]
        public async Task CreateModelInvalid()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var core = new WeatherForecastCore(context);
                var controller = new WeatherForecastController(_mockLogger.Object, core);
                var item = A.New<WeatherForecast>();
                item.Summary = "";
                controller.ModelState.AddModelError("Summary", "Required");
                var result = await controller.Create(item);
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }
    }
}
