using API.GraphQL_Chat.GraphQL.Models;
using GraphQL;
using GraphQL.Types;

namespace API.GraphQL_Chat.GraphQL.Types;

public class MessageType : ObjectGraphType<Message>
{
    public MessageType()
    {
        Field(o => o.Content);
        Field(o => o.SentAt);
        Field(o => o.From, false, typeof(MessageFromType))
            .Resolve(ResolveFrom);
    }
    
    // this is needed to resolve complext types, GraphQL wants you to tell it how to resolve the From object
    private MessageFrom ResolveFrom(IResolveFieldContext<Message> context)
    {
        var message = context.Source;
        return message.From;
    }
}