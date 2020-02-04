using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Nomadas.Network.Models;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Nomadas.Network.Tests
{
    public class WeatherForecastIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public WeatherForecastIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CanGetWeatherForecasts()
        {
            var _client = createClient();

            List<WeatherForecast> weatherforecasts = await getWeatherForecasts(_client);
            Assert.Equal(5, weatherforecasts.Count());
        }

        private async Task<List<WeatherForecast>> getWeatherForecasts(HttpClient _client)
        {
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("/weatherforecast");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            List<WeatherForecast> weatherforecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(stringResponse);
            return weatherforecasts;
        }

        [Fact]
        public async Task CanGetWeatherForecastById()
        {
            var _client = createClient();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("/weatherforecast/1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            WeatherForecast weatherforecast = JsonConvert.DeserializeObject<WeatherForecast>(stringResponse);
            Assert.Equal(1, weatherforecast.Id);
            Assert.StartsWith("Summary ", weatherforecast.Summary);
            Assert.Equal(10, weatherforecast.RandomString.Length);
            Assert.IsType(Type.GetType("System.DateTime"), weatherforecast.Created);
            Assert.Equal(DateTime.Now.AddDays(1).Day, weatherforecast.Date.Day);
        }

        [Fact]
        public async Task CanAdd1WeatherForecast()
        {
            var _client = createClient();

            string stringResponse = await add1WeatherForecast(_client);

            WeatherForecast weatherforecast = JsonConvert.DeserializeObject<WeatherForecast>(stringResponse);
            Assert.Equal(6, weatherforecast.Id);
            Assert.StartsWith("Summary", weatherforecast.Summary);
            Assert.StartsWith("RandomString", weatherforecast.RandomString);
            Assert.IsType(Type.GetType("System.DateTime"), weatherforecast.Created);
            Assert.Equal(DateTime.Now.Day, weatherforecast.Date.Day);
            Assert.Equal(DateTime.Now.Month, weatherforecast.Date.Month);
            Assert.Equal(DateTime.Now.Year, weatherforecast.Date.Year);
        }

        private async Task<string> add1WeatherForecast(HttpClient _client, string summary = "Summary")
        {
            var obj = new WeatherForecast()
            {
                RandomString = "RandomString",
                Date = DateTime.Now,
                Summary = summary,
                TemperatureC = 30
            };

            var jsonRequest = JsonConvert.SerializeObject(obj);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync("/weatherforecast", content);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return stringResponse;
        }

        [Fact]
        public async Task CanGetWeatherForecastsAfterAdded1()
        {
            var _client = createClient();

            await add1WeatherForecast(_client);

            List<WeatherForecast> weatherforecasts = await getWeatherForecasts(_client);

            Assert.Equal(6, weatherforecasts.Count());
        }

        private HttpClient createClient()
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // // Create a new service provider.
                    // var serviceProvider = new ServiceCollection()
                    //     .AddEntityFrameworkInMemoryDatabase()
                    //     .BuildServiceProvider();

                    // Add a database context (ApplicationDbContext) using an in-memory database for testing.
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        // options.UseInternalServiceProvider(serviceProvider);
                    });

                    services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
                        
                    // Build the service provider.
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database contexts    
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var appDb = scopedServices.GetRequiredService<ApplicationDbContext>();
                        // var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                        appDb.Database.EnsureDeleted();
                        appDb.Database.EnsureCreated();
                    }
                });
            }).CreateClient();
        }

        [Fact]
        public async Task CanDeleteWeatherForecastsAdded()
        {
            var _client = createClient();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync("/weatherforecast/1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            List<WeatherForecast> weatherforecasts = await getWeatherForecasts(_client);

            Assert.Equal(4, weatherforecasts.Count());
        }

        [Fact]
        public async Task CanUpdateWeatherForecasts()
        {
            var _client = createClient();

            var httpResponse = await _client.GetAsync("/weatherforecast/1");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            WeatherForecast weatherforecast = JsonConvert.DeserializeObject<WeatherForecast>(stringResponse);

            weatherforecast.Summary = "Summary Updated";
            weatherforecast.RandomString = "RandomString Updated";
            weatherforecast.TemperatureC = 10;

            var jsonRequest = JsonConvert.SerializeObject(weatherforecast);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            httpResponse = await _client.PutAsync("/weatherforecast/1", content);

            httpResponse.EnsureSuccessStatusCode();

            httpResponse = await _client.GetAsync("/weatherforecast/1");

            httpResponse.EnsureSuccessStatusCode();

            stringResponse = await httpResponse.Content.ReadAsStringAsync();
            weatherforecast = JsonConvert.DeserializeObject<WeatherForecast>(stringResponse);

            Assert.Equal(1, weatherforecast.Id);
            Assert.Equal("Summary Updated", weatherforecast.Summary);
            Assert.Equal("RandomString Updated", weatherforecast.RandomString);
            Assert.Equal(DateTime.UtcNow.Day, weatherforecast.Modified.Day);
            Assert.Equal(DateTime.UtcNow.Month, weatherforecast.Modified.Month);
            Assert.Equal(DateTime.UtcNow.Year, weatherforecast.Modified.Year);
        }

        [Fact]
        public async Task CanDeleteAllWeatherForecastsAndGetNotfound()
        {
            var _client = createClient();

            await deleteAll(_client);

            var httpResponse = await _client.GetAsync("/weatherforecast");

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        private async Task deleteAll(HttpClient _client)
        {
            var httpResponse = await _client.DeleteAsync("/weatherforecast/1");
            httpResponse.EnsureSuccessStatusCode();
            httpResponse = await _client.DeleteAsync("/weatherforecast/2");
            httpResponse.EnsureSuccessStatusCode();
            httpResponse = await _client.DeleteAsync("/weatherforecast/3");
            httpResponse.EnsureSuccessStatusCode();
            httpResponse = await _client.DeleteAsync("/weatherforecast/4");
            httpResponse.EnsureSuccessStatusCode();
            httpResponse = await _client.DeleteAsync("/weatherforecast/5");
            httpResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetBadRequestCreatingNullItem()
        {
            var _client = createClient();

            var obj = new WeatherForecast();

            var jsonRequest = JsonConvert.SerializeObject(obj);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            var httpResponse = await _client.PostAsync("/weatherforecast", content);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task GetBadRequestCreatingInvalidModel()
        {
            var _client = createClient();

            var obj = new WeatherForecast()
            {
                RandomString = "RandomString",
                Date = DateTime.Now,
                Summary = "",  //empty on porpuse
                TemperatureC = 30
            };

            var jsonRequest = JsonConvert.SerializeObject(obj);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            var httpResponse = await _client.PostAsync("/weatherforecast", content);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task GetNotFoundSearchingANonExistingItem()
        {
            var _client = createClient();

            var httpResponse = await _client.GetAsync("/weatherforecast/6");

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task GetOrderedItems()
        {
            var _client = createClient();

            await deleteAll(_client);
            await add1WeatherForecast(_client, "Three");
            await add1WeatherForecast(_client, "Two");
            await add1WeatherForecast(_client, "One");

            var httpResponse = await _client.GetAsync("/weatherforecast/ordered");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            List<WeatherForecast> weatherforecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(stringResponse);

            Assert.Equal("One", weatherforecasts[0].Summary);
            Assert.Equal("Three", weatherforecasts[1].Summary);
            Assert.Equal("Two", weatherforecasts[2].Summary);
        }
    }
}
