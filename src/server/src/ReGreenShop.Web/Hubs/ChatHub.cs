using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Web.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> connectedUsers = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var userName = Context.User?.Identity?.Name ?? "Unknown";
        var isAdmin = Context.User?.IsInRole(AdminName) ?? false;

        if (!string.IsNullOrEmpty(userId) && !isAdmin)
        {
            connectedUsers[userId] = userName;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            connectedUsers.TryRemove(userId, out _);
        }
        return base.OnDisconnectedAsync(exception);
    }

    public Task<List<UserDto>> GetConnectedUsers()
    {
        var isAdmin = Context.User?.IsInRole(AdminName) ?? false;
        if (!isAdmin)
        {
            return Task.FromResult(new List<UserDto>());
        }

        var users = connectedUsers.Select(kvp => new UserDto
        {
            UserId = kvp.Key,
            UserName = kvp.Value
        }).ToList();

        return Task.FromResult(users);
    }

    public Task SendMessage(string receiverUserId, string message)
    {
        var senderUserId = Context.UserIdentifier;
        var userName = Context.User?.Identity?.Name;
        return Clients.User(receiverUserId)
                      .SendAsync("ReceiveMessage", senderUserId, userName, message);
    }

    public class UserDto
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
