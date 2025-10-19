# Mutation
http://localhost:5147/graphql
```
mutation {
  addMessage(message: {
    fromId: "user123",
    content: "Hello from GraphQL!",
    sentAt: "2025-10-11T14:30:00"
  }) {
    content
    sentAt
    from {
      id
      displayName
    }
  }
}

```

# Subscription
ws://localhost:5147/graphql
```
subscription {
  messageAdded {
    content
    sentAt
  }
}
```

# Docker
Run docker compose file
Web is on 4200 (http)
Api is on 5147 (http)