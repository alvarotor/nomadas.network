using GraphQL;
using GraphQL.Types;

public class AppSchema : Schema
{
    public AppSchema(IDependencyResolver resolver) : base(resolver)
    {
        Query = resolver.Resolve<AppQuery>();
    }
}