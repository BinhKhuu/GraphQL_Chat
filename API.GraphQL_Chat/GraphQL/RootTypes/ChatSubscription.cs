using System.Reactive.Linq;
using API.GraphQL_Chat.GraphQL.Models;
using API.GraphQL_Chat.Interfaces;
using GraphQL;
using GraphQL.Types;
using GraphQL.Validation.Rules;

namespace API.GraphQL_Chat.GraphQL.Types;

public class ChatSubscription : ObjectGraphType
{
    private readonly IChat _chat;
    
    public ChatSubscription(IChat chat)
    {
        _chat = chat;
        Field<MessageType, Message>("messageAdded")
            .ResolveStream(Subscribe);

        Field<MessageType, Message>("messageAddedByUser")
            .Argument<NonNullGraphType<StringGraphType>>("id")
            .ResolveStream(SubscribeById);

        Field<MessageType, Message>("messageAddedAsync")
            .ResolveStreamAsync(SubscribeAsync);

        Field<MessageType, Message>("messageAddedByUserAsync")
            .Argument<NonNullGraphType<StringGraphType>>("id")
            .ResolveStreamAsync(SubscribeByIdAsync);

        Field<ListGraphType<MessageType>, List<Message>>("messageGetAll")
            .ResolveStream(_ => _chat.MessagesGetAll());

        Field<StringGraphType>("newMessageContent")
            .ResolveStream(context => Subscribe(context).Select(message => message.Content));

        int counter = 0;
        Field<IntGraphType, int>("messageCounter")
            .ResolveStream(context => Subscribe(context).Select(_ => ++counter));
    }

    private IObservable<Message> SubscribeById(IResolveFieldContext context)
    {
        string id = context.GetArgument<string>("id");

        var messages = _chat.Messages();

        return messages.Where(message => message.From.Id == id);
    }

    private async Task<IObservable<Message?>> SubscribeByIdAsync(IResolveFieldContext context)
    {
        string id = context.GetArgument<string>("id");

        var messages = await _chat.MessagesAsync().ConfigureAwait(false);
        return messages.Where(message => message.From.Id == id);
    }

    private IObservable<Message> Subscribe(IResolveFieldContext context)
    {
        return _chat.Messages();
    }

    private async Task<IObservable<Message?>> SubscribeAsync(IResolveFieldContext context)
    {
        return await _chat.MessagesAsync().ConfigureAwait(false);
    }

}