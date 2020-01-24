using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Nomadas.Network.Tests
{
    public class BasicTests
    {
        private HttpClient _client;

        public BasicTests()
        {
            Task.Run(async ()=> await InitializeServer()).Wait();
        }

        private async Task InitializeServer()
        {
            // Arrange
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>();
                });

            // Create and start up the host
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient which is setup for the test host
            _client = host.GetTestClient();
        }

        [Fact]
        public async Task HomeShouldReturnHelloWorld()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello world", responseString);
        }

        [Fact]
        public async Task HealthShouldReturnOK()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("OK", responseString);
        }

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        // [Fact]
        // public void FailingTest()
        // {
        //     Assert.Equal(5, Add(2, 2));
        // }

        int Add(int x, int y) => x + y;

        [Theory]
        [InlineData(2, 5)]
        [InlineData(4, 9)]
        [InlineData(15, 42)]
        void SumOfPositiveNumbersIsPositive(int a, int b)
        {
            Assert.True(a + b > 0);
        }
    }
}
