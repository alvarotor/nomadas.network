using GraphQL.Types;

namespace Nomadas.Network.GraphQL
{
    public class WeatherForecastInputType : InputObjectGraphType
    {
        public WeatherForecastInputType()
        {
            Name = "WeatherForecastInput";
            Field<DateGraphType>("date");
            Field<IntGraphType>("temperatureC");
            Field<NonNullGraphType<StringGraphType>>("summary");
            Field<StringGraphType>("randomString");
        }
    }
}