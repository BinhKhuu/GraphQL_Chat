using API.GraphQL_Chat.GraphQL.Types;
using API.GraphQL_Chat.Interfaces;

namespace API.GraphQL_Chat.GraphQL.Schema;

public class ChatSchema : global::GraphQL.Types.Schema
{
    public ChatSchema(IChat chat)
    {
        Query = new ChatQuery(chat);
        Mutation = new ChatMutation(chat);
        Subscription = new ChatSubscription(chat);
    }
}