using GraphQL.Types;
using Nomadas.Network.Models;

namespace Nomadas.Network.GraphQL
{
    public class WeatherForecastType : ObjectGraphType<WeatherForecast>
    {
        public WeatherForecastType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id property from the WeatherForecast object.");
            Field(x => x.Created, type: typeof(DateGraphType)).Description("Created property from the WeatherForecast object.");
            Field(x => x.Modified, type: typeof(DateGraphType)).Description("Modified property from the WeatherForecast object.");
            Field(x => x.Date, type: typeof(DateGraphType)).Description("Date property from the WeatherForecast object.");
            Field(x => x.TemperatureC, type: typeof(IntGraphType)).Description("TemperatureC property from the WeatherForecast object.");
            Field(x => x.TemperatureF, type: typeof(IntGraphType)).Description("TemperatureF property from the WeatherForecast object.");
            Field(x => x.Summary, type: typeof(StringGraphType)).Description("Summary property from the WeatherForecast object.");
            Field(x => x.RandomString, type: typeof(StringGraphType)).Description("RandomString property from the WeatherForecast object.");
        }
    }
}