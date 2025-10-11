using System.Collections.Concurrent;
using API.GraphQL_Chat.GraphQL.Models;

namespace API.GraphQL_Chat.Interfaces;

public interface IChat
{
    ConcurrentStack<Message> AllMessages { get; }
    Message AddMessage(Message message);
    IObservable<Message> Messages();
    IObservable<List<Message>> MessagesGetAll();
    Message AddMessage(ReceivedMessage message);
    Task<IObservable<Message>> MessagesAsync();

}