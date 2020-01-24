using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class BasicTests
    {
        public string ApiBaseUrl { get; set; } = "http://locahost:5000";

        [Fact]
        public async Task PassingEndPointTest()
        {
            var apiClient = new HttpClient();

            var apiResponse = await apiClient.GetAsync($"{ApiBaseUrl}/health");

            Assert.True(apiResponse.IsSuccessStatusCode);

            var stringResponse = await apiResponse.Content.ReadAsStringAsync();

            Assert.Equal("OK", stringResponse);
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
    }
}
