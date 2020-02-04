using System.Threading.Tasks;
using Nomadas.Network.Models;
using Xunit;
using Nomadas.Network.Core;
using Microsoft.EntityFrameworkCore;
using GenFu;
using System.Linq;
using System;

namespace Nomadas.Network.Tests
{
    public class WeatherForecastCoreUnitTests
    {
        private readonly DbContextOptions<ApplicationDbContext> options;

        public WeatherForecastCoreUnitTests()
        {
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
        public async Task FindAll()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var service = new WeatherForecastCore(context);
                var result = await service.FindAll();
                Assert.Equal(25, result.Count());
            }
        }
    }
}
