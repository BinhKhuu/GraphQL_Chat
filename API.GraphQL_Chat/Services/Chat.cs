using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using API.GraphQL_Chat.GraphQL.Models;
using API.GraphQL_Chat.Interfaces;

namespace API.GraphQL_Chat.Services;

public class Chat : IChat
{
    // Latest message stream
    private readonly ISubject<Message> _messageStream = new ReplaySubject<Message>(1);
    
    // All Messages stream
    private readonly ISubject<List<Message>> _allMessageStream = new ReplaySubject<List<Message>>(1);
    public ConcurrentDictionary<string, string> Users { get; set; }
    public ConcurrentStack<Message> AllMessages { get; }
    
    public Chat()
    {
        AllMessages = new ConcurrentStack<Message>();
        Users = new ConcurrentDictionary<string, string>()
        {
            ["1"] = "developer",
            ["2"] = "tester"
        };
    }

    public Message AddMessage(ReceivedMessage message)
    {
        if (!Users.TryGetValue(message.FromId, out string? displayName))
        {
            displayName = "(unknown)";
        }

        return AddMessage(new Message
        {
            Content = message.Content,
            SentAt = message.SentAt,
            From = new MessageFrom
            {
                DisplayName = displayName,
                Id = message.FromId
            }
        });
    }
    
    // Add and broadcast new message event
    public Message AddMessage(Message message)
    {
        AllMessages.Push(message);
        _messageStream.OnNext(message);
        return message;
    }
    
    // subscription event to get latest message
    public IObservable<Message> Messages()
    {
        return _messageStream.AsObservable();
    }

    // subscription event to get all messages
    public IObservable<List<Message>> MessagesGetAll()
    {
        return _allMessageStream.AsObservable();
    }
    
    // Async subscription event tet get latest message
    public async Task<IObservable<Message>> MessagesAsync()
    {
        await Task.Delay(100);
        return _messageStream.AsObservable();
    }
    
    public void AddError(Exception exception)
    {
        _messageStream.OnError(exception);
    }
}