using Commons.Models;
using Helpers;

namespace Repositories.Managers;

public class MockUwcDbContext
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
    }

    public void Set<T>(IEnumerable<T> newList) where T : IndexedEntity
    {
        switch (typeof(T))
        {
            case { } account when account == typeof(Account):
                AccountTable = (newList as List<Account>)!;
                break;
            case { } userProfile when userProfile == typeof(UserProfile):
                UserProfileTable = (newList as List<UserProfile>)!;
                break;
            case { } mcpData when mcpData == typeof(McpData):
                McpDataTable = (newList as List<McpData>)!;
                break;
            case { } mcpEmptyRecord when mcpEmptyRecord == typeof(McpEmptyRecord):
                McpEmptyRecordTable = (newList as List<McpEmptyRecord>)!;
                break;
            case { } mcpFillLevelLog when mcpFillLevelLog == typeof(McpFillLevelLog):
                McpFillLevelLogTable = (newList as List<McpFillLevelLog>)!;
                break;
            case { } taskData when taskData == typeof(TaskData):
                TaskDataTable = (newList as List<TaskData>)!;
                break;
            case { } vehicleData when vehicleData == typeof(VehicleData):
                VehicleDataTable = (newList as List<VehicleData>)!;
                break;
            case { } message when message == typeof(Message):
                MessageTable = (newList as List<Message>)!;
                break;
        }
    }

    public List<T> Set<T>() where T : IndexedEntity
    {
        return typeof(T) switch
        {
            { } account when account == typeof(Account) => AccountTable as List<T>,
            { } userProfile when userProfile == typeof(UserProfile) => UserProfileTable as List<T>,
            { } mcpData when mcpData == typeof(McpData) => McpDataTable as List<T>,
            { } mcpEmptyRecord when mcpEmptyRecord == typeof(McpEmptyRecord) => McpEmptyRecordTable as List<T>,
            { } mcpFillLevelLog when mcpFillLevelLog == typeof(McpFillLevelLog) => McpFillLevelLogTable as List<T>,
            { } taskData when taskData == typeof(TaskData) => TaskDataTable as List<T>,
            { } vehicleData when vehicleData == typeof(VehicleData) => VehicleDataTable as List<T>,
            { } message when message == typeof(Message) => MessageTable as List<T>,
            _ => new List<T>()
        } ?? throw new InvalidOperationException($"Retrieving data for type {typeof(T).Name} is not supported in this mock context.");
    }
}