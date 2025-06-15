using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Web.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> connectedUsers = new();
    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!connectedUsers.ContainsKey(userId!))
        {
            connectedUsers[userId!] = Context.ConnectionId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        connectedUsers.Remove(userId!);
        return base.OnDisconnectedAsync(exception);
    }

    public Task<List<string>> GetConnectedUsers()
    {
        var isAdmin = Context.User?.IsInRole(AdminName) ?? false;
        if (!isAdmin)
        {
            return Task.FromResult(new List<string>());
        }

        return Task.FromResult(connectedUsers.Keys.ToList());
    }

    public Task SendMessage(string receiverUserId, string message)
    {
        var senderUserId = Context.UserIdentifier;
        var userName = Context.User?.Identity?.Name;
        return Clients.User(receiverUserId)
                      .SendAsync("ReceiveMessage", senderUserId,userName, message);
    }
}
