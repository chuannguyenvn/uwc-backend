using Commons.Models;

namespace Repositories.Managers;

public interface ISeedable
{
    public void AddAccount (Account entry);
    public void AddUserProfile (UserProfile entry);
    public void AddMcpData (McpData entry);
    public void AddMcpEmptyRecord (McpEmptyRecord entry);
    public void AddMcpFillLevelLog (McpFillLevelLog entry);
    public void AddVehicleData (VehicleData entry);
    public void AddTaskData (TaskData entry);
    public void AddMessage (Message entry);
    public void Complete();
}