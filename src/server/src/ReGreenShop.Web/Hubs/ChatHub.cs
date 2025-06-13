using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace ReGreenShop.Web.Hubs;
[Authorize]
public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> userConnections = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            lock (userConnections)
            {
                userConnections[userId] = Context.ConnectionId;
            }
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.FindFirst("nameid")?.Value;
        if (userId != null)
        {
            lock (userConnections)
            {
                userConnections.Remove(userId);
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    public Task SendMessage(string receiverUserId, string message)
    {
        var senderUserId = Context.User.FindFirst("nameid")?.Value;

        if (receiverUserId != null && userConnections.TryGetValue(receiverUserId, out var connectionId))
        {
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", senderUserId, message);
        }

        return Task.CompletedTask;
    }
}
