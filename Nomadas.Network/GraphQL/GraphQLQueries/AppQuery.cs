using GraphQL.Types;
using Nomadas.Network.Core;

public class AppQuery : ObjectGraphType
{
    public AppQuery(IWeatherForecastCore repository)
    {
        Field<ListGraphType<WeatherForecastType>>(
           "weatherforecasts",
           resolve: context => repository.FindAll()
        );
    }
}