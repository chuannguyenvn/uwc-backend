using Commons.Models;

namespace Repositories.Managers;

public class MockUwcDbContext : ISeedable
{
    public List<Account> AccountTable { get; set; }
    public List<UserProfile> UserProfileTable { get; set; }
    public List<McpData> McpDataTable { get; set; }
    public List<McpEmptyRecord> McpEmptyRecordTable { get; set; }
    public List<McpFillLevelLog> McpFillLevelLogTable { get; set; }
    public List<TaskData> TaskDataTable { get; set; }
    public List<VehicleData> VehicleDataTable { get; set; }
    public List<Message> MessageTable { get; set; }

    public MockUwcDbContext()
    {
        AccountTable = new List<Account>();
        UserProfileTable = new List<UserProfile>();
        McpDataTable = new List<McpData>();
        McpEmptyRecordTable = new List<McpEmptyRecord>();
        McpFillLevelLogTable = new List<McpFillLevelLog>();
        TaskDataTable = new List<TaskData>();
        VehicleDataTable = new List<VehicleData>();
        MessageTable = new List<Message>();

        // TODO: Add some mock data here
    }

    public void Set<T>(IEnumerable<T> newList) where T : IndexedEntity
    {
        switch (typeof(T))
        {
            case { } account when account == typeof(Account):
                AccountTable = newList as List<Account>;
                break;
            case { } mcpData when mcpData == typeof(McpData):
                McpDataTable = newList as List<McpData>;
                break;
            case { } vehicleData when vehicleData == typeof(VehicleData):
                VehicleDataTable = newList as List<VehicleData>;
                break;
            case { } message when message == typeof(Message):
                MessageTable = newList as List<Message>;
                break;
        }
    }

    public List<T> Set<T>() where T : IndexedEntity
    {
        return typeof(T) switch
        {
            { } account when account == typeof(Account) => AccountTable as List<T>,
            { } mcpData when mcpData == typeof(McpData) => McpDataTable as List<T>,
            { } vehicleData when vehicleData == typeof(VehicleData) => VehicleDataTable as List<T>,
            { } message when message == typeof(Message) => MessageTable as List<T>,
            _ => new List<T>()
        } ?? throw new InvalidOperationException();
    }

    public void AddAccount(Account entry)
    {
        AccountTable.Add(entry);
    }

    public void AddUserProfile(UserProfile entry)
    {
        UserProfileTable.Add(entry);
    }

    public void AddMcpData(McpData entry)
    {
        McpDataTable.Add(entry);
    }

    public void AddMcpEmptyRecord(McpEmptyRecord entry)
    {
        McpEmptyRecordTable.Add(entry);
    }

    public void AddMcpFillLevelLog(McpFillLevelLog entry)
    {
        McpFillLevelLogTable.Add(entry);
    }

    public void AddVehicleData(VehicleData entry)
    {
        VehicleDataTable.Add(entry);
    }

    public void AddTaskData(TaskData entry)
    {
        TaskDataTable.Add(entry);
    }

    public void AddMessage(Message entry)
    {
        MessageTable.Add(entry);
    }

    public void Complete()
    {
        
    }
}