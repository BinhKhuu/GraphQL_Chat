using GraphQL.Types;

namespace API.GraphQL_Chat.GraphQL.Types;

public class MessageInputType : InputObjectGraphType
{
    public MessageInputType()
    {
        Field<StringGraphType>("fromId");
        Field<StringGraphType>("content");
        Field<DateTimeOffsetGraphType>("sentAt");
    }
}