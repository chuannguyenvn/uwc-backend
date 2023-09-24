using Commons.Categories;
using Microsoft.AspNetCore.SignalR;

namespace Hubs;

public class ScreenTrackingHub : Hub
{
    private static readonly Dictionary<string, Screen> ActiveScreens = new();

    public void SetActiveScreen(Screen screen)
    {
        // Set the active screen for the current user/connection
        var connectionId = Context.ConnectionId;
        ActiveScreens[connectionId] = screen;
    }

    public void SendUpdateToActiveScreen(Screen screen, string message)
    {
        var connectionId = Context.ConnectionId;
        if (ActiveScreens.TryGetValue(connectionId, out var activeScreen) && activeScreen == screen)
        {
            // Send the update to the client on the active screen
            Clients.Client(connectionId).SendAsync("ReceiveUpdate", message);
        }
        else
        {
            // The client is not on the active screen; you can handle this case accordingly
        }
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        // Remove the disconnected client from the active screens dictionary
        ActiveScreens.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}