using API.GraphQL_Chat.Interfaces;
using GraphQL.Server.Transports.AspNetCore.WebSockets.GraphQLWs;
using GraphQL.Types;

namespace API.GraphQL_Chat.GraphQL.Types;

public class ChatQuery : ObjectGraphType
{
    public ChatQuery(IChat chat)
    {
        Field<ListGraphType<MessageType>>("messages")
            .Resolve(_ => chat.AllMessages.Take(100));

    }
}