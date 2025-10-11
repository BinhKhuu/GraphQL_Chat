using API.GraphQL_Chat.GraphQL.Schema;
using API.GraphQL_Chat.Interfaces;
using API.GraphQL_Chat.Services;
using GraphQL;
using GraphQL.Server;

namespace API.GraphQL_Chat.BuilderConfigurators;

public class GraphQLBuilderConfigurator
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IChat, Chat>();
        // GraphQL.MicrosoftDIGraphQLBuilderExtensions
        builder.Services.AddGraphQL(options => 
            options.AddSchema<ChatSchema>()
                .AddSystemTextJson() // use system text because it handles async io better than the newtonsoft extension
            );
        
    }
}