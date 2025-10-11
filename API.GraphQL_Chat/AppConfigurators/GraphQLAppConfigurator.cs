namespace API.GraphQL_Chat.AppConfigurators;

public class GraphQLAppConfigurator
{
    public static void Configure(WebApplication app)
    {
        app.MapGraphQL("/graphql");
        app.UseWebSockets();
        app.UseGraphQLAltair();
    }
}