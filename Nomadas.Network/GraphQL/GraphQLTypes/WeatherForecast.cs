using GraphQL.Types;
using Nomadas.Network.Models;

public class WeatherForecastType : ObjectGraphType<WeatherForecast>
{
    public WeatherForecastType()
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("Id property from the WeatherForecast object.");
        Field(x => x.Created).Description("Created property from the WeatherForecast object.");
        Field(x => x.Modified).Description("Modified property from the WeatherForecast object.");
        Field(x => x.Date).Description("Date property from the WeatherForecast object.");
        Field(x => x.TemperatureC).Description("TemperatureC property from the WeatherForecast object.");
        Field(x => x.TemperatureF).Description("TemperatureC property from the WeatherForecast object.");
        Field(x => x.Summary).Description("TemperatureC property from the WeatherForecast object.");
        Field(x => x.RandomString).Description("TemperatureC property from the WeatherForecast object.");
    }
}