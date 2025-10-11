namespace API.GraphQL_Chat.GraphQL.Models;

public class Message
{
    public MessageFrom From { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
}