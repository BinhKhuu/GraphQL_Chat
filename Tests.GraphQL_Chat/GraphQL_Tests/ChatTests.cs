using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.GraphQL_Chat.GraphQL_Tests;

public class ChatTests : IClassFixture<WebApplicationFactory<API.GraphQL_Chat.Program>>
{
    private readonly HttpClient _client;
    private WebApplicationFactory<API.GraphQL_Chat.Program> _factory;
    public ChatTests(WebApplicationFactory<API.GraphQL_Chat.Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    private object mutationRequest = new
    {
        query = @"mutation {
            addMessage(message: {
                fromId: ""user123"",
                content: ""Hello from GraphQL!"",
                sentAt: ""2025-10-11T14:30:00""
            }) {
                content
                sentAt
                from {
                    id
                    displayName
                }
            }
        }"
    };

    private object subscriptionRequest = new
    {
        Query = @"subscription {
          messageAdded {
            content
            sentAt
          }
        }"
    };

    [Fact]
    public async Task SuccessFullMutationRequest()
    {
        var response = await _client.PostAsJsonAsync("/graphql",mutationRequest);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Hello from GraphQL!", content);
    }
    
    [Fact]
    public async Task InvalidQuery_ReturnsError()
    {
        var gqlRequest = new { query = "{ unknownField }" };
        var response = await _client.PostAsJsonAsync("/graphql", gqlRequest);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Cannot query field", content);
    }

    private async Task<WebSocket> InitSubscription_Assert()
    {
        _factory = new WebApplicationFactory<API.GraphQL_Chat.Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.UseKestrel(); // Enables full WebSocket support
        });
        
        // Create websocket client
        var webSocketClient = _factory.Server.CreateWebSocketClient();
        webSocketClient.ConfigureRequest = req => req.Headers.Add("sec-websocket-protocol", "graphql-ws");
        
        // Send connection_init
        var subscriptionInitRequest = Encoding.UTF8.GetBytes(
            "{\"type\":\"connection_init\",\"payload\":{}}"
        );
        var webSocket = await webSocketClient.ConnectAsync(new Uri("ws://localhost/graphql"), CancellationToken.None);
        await webSocket.SendAsync(subscriptionInitRequest, WebSocketMessageType.Text, true, CancellationToken.None);

        var initMessageBuffer = new byte[1024 * 4];
        var initMessageResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(initMessageBuffer), CancellationToken.None);
        var initMessage = Encoding.UTF8.GetString(initMessageBuffer, 0, initMessageResult.Count);
        
        Assert.Contains("connection_ack", initMessage);

        return webSocket;
    }

    private async Task<string> WaitForSubscriptionMessage(WebSocket webSocket, string waitForMessage)
    {
        var tcs = new TaskCompletionSource<string>();

        _ = Task.Run(async () =>
        {
            var buffer = new byte[4096];
            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (message.Contains(waitForMessage))
                {
                    tcs.SetResult(message);
                    break;
                }
            }
        });
        
        return await tcs.Task; // waits until message arrives
    }
    
    [Fact]
    public async Task ValidSubscriptionRequest()
    {
        var webSocket = await InitSubscription_Assert();
        // queue message with mutation request
        var client = _factory.Server.CreateClient();
        var response = await client.PostAsJsonAsync("/graphql", mutationRequest);
        response.EnsureSuccessStatusCode();
        var mutationContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Hello from GraphQL!", mutationContent);

        var subscriptionRequest = @"subscription {
          messageAdded {
            content
            sentAt
          }
        }";
        
        var subscriptionRequestPayload = new
        {
            id = "1",
            type = "start",
            payload = new
            {
                query = subscriptionRequest
            }
        };

        var subscriptionStartBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(subscriptionRequestPayload));
        await webSocket.SendAsync(subscriptionStartBuffer, WebSocketMessageType.Text, true, CancellationToken.None);

        var subscriptionResultBuffer = new byte[1024 * 4];

        var subscriptionResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(subscriptionResultBuffer), CancellationToken.None);
        var message = Encoding.UTF8.GetString(subscriptionResultBuffer, 0, subscriptionResult.Count);
        
        var received = await WaitForSubscriptionMessage(webSocket, "messageAdded");
        Assert.Contains("Hello from GraphQL!", received);
        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
    }

    [Fact]
    public async Task InvalidSubscriptionRequest_ReturnsError()
    {
        var webSocket = await InitSubscription_Assert();
        var client = _factory.Server.CreateClient();
        
        var response = await client.PostAsJsonAsync("/graphql", mutationRequest);
        response.EnsureSuccessStatusCode();
        var mutationContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Hello from GraphQL!", mutationContent);
        
        var subscriptionRequest = @"subscription {
          messageAdded {
            content
            sentAt
            unknownProperty
          }
        }";
        
        var subscriptionRequestPayload = new
        {
            id = "1",
            type = "start",
            payload = new
            {
                query = subscriptionRequest
            }
        };

        var subscriptionStartBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(subscriptionRequestPayload));
        await webSocket.SendAsync(subscriptionStartBuffer, WebSocketMessageType.Text, true, CancellationToken.None);

        var subscriptionResultBuffer = new byte[1024 * 4];

        var subscriptionResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(subscriptionResultBuffer), CancellationToken.None);
        var message = Encoding.UTF8.GetString(subscriptionResultBuffer, 0, subscriptionResult.Count);
        
        var received = await WaitForSubscriptionMessage(webSocket, "errors");
        Assert.Contains("errors", received);
    }

}

