using API.GraphQL_Chat.GraphQL.Models;
using API.GraphQL_Chat.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace API.GraphQL_Chat.GraphQL.Types;

public class ChatMutation : ObjectGraphType
{
    public ChatMutation(IChat chat)
    {
        Field<MessageType>("addMessage")
            .Argument<MessageInputType>("message")
            .Resolve(context =>
            {
                var receivedMessage = context.GetArgument<ReceivedMessage>("message");
                var message = chat.AddMessage(receivedMessage);
                return message;
            });
    }
}