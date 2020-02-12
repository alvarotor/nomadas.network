using GraphQL;
using GraphQL.Types;
using Nomadas.Network.Core;
using Nomadas.Network.Models;

namespace Nomadas.Network.GraphQL
{
    public class AppMutation : ObjectGraphType
    {
        public AppMutation(IWeatherForecastCore repository)
        {
            Field<WeatherForecastType>(
                "createWeatherForecast",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<WeatherForecastInputType>> { Name = "weatherforecast" }),
                resolve: context =>
                {
                    var weatherforecast = context.GetArgument<WeatherForecast>("weatherforecast");
                    return repository.Create(weatherforecast);
                }
            );

            FieldAsync<WeatherForecastType>(
                "updateWeatherForecast",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<WeatherForecastInputType>> { Name = "weatherforecast" },
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "weatherforecastId" }),
                resolve: async context =>
                {
                    var weatherforecast = context.GetArgument<WeatherForecast>("weatherforecast");
                    var weatherforecastId = context.GetArgument<long>("weatherforecastId");

                    var dbWeatherForecast = await repository.GetById(weatherforecastId);
                    if (dbWeatherForecast == null)
                    {
                        context.Errors.Add(new ExecutionError("Couldn't find weatherforecast in db."));
                        return null;
                    }

                    dbWeatherForecast.RandomString =
                        string.IsNullOrEmpty(weatherforecast.RandomString) ? dbWeatherForecast.RandomString : weatherforecast.RandomString;
                    dbWeatherForecast.Summary =
                        string.IsNullOrEmpty(weatherforecast.Summary) ? dbWeatherForecast.Summary : weatherforecast.Summary;
                    dbWeatherForecast.TemperatureC =
                        string.IsNullOrEmpty(weatherforecast.TemperatureC.ToString()) ? dbWeatherForecast.TemperatureC : weatherforecast.TemperatureC;

                    return repository.Update(dbWeatherForecast);
                }
            );
            
            FieldAsync<StringGraphType>(
                "deleteOwner",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "weatherforecastId" }),
                resolve: async context =>
                {
                    var weatherforecastId = context.GetArgument<long>("weatherforecastId");
                    var weatherforecast = await repository.GetById(weatherforecastId);
                    if (weatherforecast == null)
                    {
                        context.Errors.Add(new ExecutionError("Couldn't find weatherforecast in db."));
                        return null;
                    }

                    await repository.Delete(weatherforecast);
                    return $"The weatherforecast with the id: {weatherforecastId} has been successfully deleted from db.";
                }
            );
        }
    }
}