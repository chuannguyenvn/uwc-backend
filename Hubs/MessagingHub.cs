using Microsoft.AspNetCore.SignalR;

namespace Hubs;

public class MessagingHub : Hub
{
    public Task ReceiveMessage(string code)
    {
        return Clients.All.SendAsync("ReceiveMessage", code);
    }
}