using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Hubs;

[Authorize]
public class BaseHub : Hub
{
    public static Dictionary<int, string> ConnectionIds = new();

    public override Task OnConnectedAsync()
    {
        var userId = int.Parse(Context.User.FindFirstValue("id"));
        ConnectionIds[userId] = Context.ConnectionId;
        Console.WriteLine("[" + Context.User.FindFirstValue("id") + "|" + Context.ConnectionId + "] connected to messaging hub");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = int.Parse(Context.User.FindFirstValue("id"));
        if (ConnectionIds.ContainsKey(userId)) ConnectionIds.Remove(userId);
        Console.WriteLine("[" + Context.User.FindFirstValue("id") + "|" + Context.ConnectionId + "] disconnected from messaging hub");
        return base.OnDisconnectedAsync(exception);
    }
}