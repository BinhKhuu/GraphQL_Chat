using API.GraphQL_Chat.GraphQL.Schema;
using GraphQL.Server;
using GraphQL.Types;

namespace API.GraphQL_Chat.AppConfigurators;

public class GraphQLAppConfigurator
{
    public static void Configure(WebApplication app)
    {
        app.UseWebSockets();
        app.MapGraphQL("/graphql");
        
        app.UseGraphQLAltair();
    }
}