using API.GraphQL_Chat.GraphQL.Models;
using GraphQL.Types;

namespace API.GraphQL_Chat.GraphQL.Types;

public class MessageFromType : ObjectGraphType<MessageFrom>
{
    public MessageFromType()
    {
        Field(o => o.Id);
        Field(o => o.DisplayName);
    }
}