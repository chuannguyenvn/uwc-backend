using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Hubs;

[Authorize]
public class MessagingHub : Hub
{
    public static Dictionary<int, string> ConnectionIds = new();

    public override Task OnConnectedAsync()
    {
        ConnectionIds.Add(int.Parse(Context.User.FindFirstValue("id")), Context.ConnectionId);
        Console.WriteLine("[" + Context.User.FindFirstValue("id") + "|" + Context.ConnectionId + "] connected to messaging hub");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        ConnectionIds.Remove(int.Parse(Context.User.FindFirstValue("id")));
        Console.WriteLine("[" + Context.User.FindFirstValue("id") + "|" + Context.ConnectionId + "] disconnected from messaging hub");
        return base.OnDisconnectedAsync(exception);
    }
}