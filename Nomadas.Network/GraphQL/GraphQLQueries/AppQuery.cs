using GraphQL;
using GraphQL.Types;
using Nomadas.Network.Core;

public class AppQuery : ObjectGraphType
{
    public AppQuery(IWeatherForecastCore repository)
    {
        Field<ListGraphType<WeatherForecastType>>("weatherforecasts", resolve: context => repository.FindAll());
        Field<WeatherForecastType>(
            "weatherforecast",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "weatherforecastId" }),
            resolve: context =>
            {
                long id;
                if (!long.TryParse(context.GetArgument<long>("weatherforecastId").ToString(), out id))
                {
                    context.Errors.Add(new ExecutionError("Wrong value for Id"));
                    return null;
                }
                return repository.GetById(id);
            }
        );
    }
}