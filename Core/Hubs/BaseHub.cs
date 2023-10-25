using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Hubs;

[Authorize]
public class BaseHub : Hub
{
    public static Dictionary<int, string> ConnectionIds = new();
    
    public static event Action<int> AccountIdConnected;
    public static event Action<int> AccountIdDisconnected;
    
    public static bool IsAccountOnline(int accountId) => ConnectionIds.ContainsKey(accountId);

    public override Task OnConnectedAsync()
    {
        var userId = int.Parse(Context.User.FindFirstValue("id"));
        ConnectionIds[userId] = Context.ConnectionId;
        Console.WriteLine("[" + Context.User.FindFirstValue("id") + "|" + Context.ConnectionId + "] connected to messaging hub");
        AccountIdConnected?.Invoke(userId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = int.Parse(Context.User.FindFirstValue("id"));
        if (ConnectionIds.ContainsKey(userId)) ConnectionIds.Remove(userId);
        Console.WriteLine("[" + Context.User.FindFirstValue("id") + "|" + Context.ConnectionId + "] disconnected from messaging hub");
        AccountIdDisconnected?.Invoke(userId);
        return base.OnDisconnectedAsync(exception);
    }
}