using Commons.Models;

namespace Repositories.Managers;

public class MockUwcDbContext
{
    public List<Account> Accounts { get; set; }
    public List<McpData> McpData { get; set; }
    public List<VehicleData> VehicleData { get; set; }
    public List<Message> Messages { get; set; }

    public MockUwcDbContext()
    {
        Accounts = new List<Account>();
        McpData = new List<McpData>();
        VehicleData = new List<VehicleData>();
        Messages = new List<Message>();

        // TODO: Add some mock data here
    }

    public void Set<T>(IEnumerable<T> newList) where T : IndexedEntity
    {
        switch (typeof(T))
        {
            case { } account when account == typeof(Account):
                Accounts = newList as List<Account>;
                break;
            case { } mcpData when mcpData == typeof(McpData):
                McpData = newList as List<McpData>;
                break;
            case { } vehicleData when vehicleData == typeof(VehicleData):
                VehicleData = newList as List<VehicleData>;
                break;
            case { } message when message == typeof(Message):
                Messages = newList as List<Message>;
                break;
        }
    }

    public List<T> Set<T>() where T : IndexedEntity
    {
        return typeof(T) switch
        {
            { } account when account == typeof(Account) => Accounts as List<T>,
            { } mcpData when mcpData == typeof(McpData) => McpData as List<T>,
            { } vehicleData when vehicleData == typeof(VehicleData) => VehicleData as List<T>,
            { } message when message == typeof(Message) => Messages as List<T>,
            _ => new List<T>()
        } ?? throw new InvalidOperationException();
    }
}