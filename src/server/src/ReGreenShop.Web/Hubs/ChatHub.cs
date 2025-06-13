using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ReGreenShop.Web.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public Task SendMessage(string receiverUserId, string message)
    {
        var senderUserId = Context.UserIdentifier;  

        return Clients.User(receiverUserId)
                      .SendAsync("ReceiveMessage", senderUserId, message);
    }
}
