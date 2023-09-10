using Commons.Models;
using Commons.Types;
using Repositories;

namespace Helpers;

public class DatabaseSeeder
{
    private readonly UwcDbContext _uwcDbContext;

    public DatabaseSeeder(UwcDbContext uwcDbContext)
    {
        _uwcDbContext = uwcDbContext;
        _uwcDbContext.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        _uwcDbContext.Database.EnsureDeleted();
        _uwcDbContext.Database.EnsureCreated();
    }

    public void SeedMcpData()
    {
        _uwcDbContext.Set<McpData>().Add(new McpData()
        {
            Address = "Placeholder address",
            Capacity = 100,
            Coordinate = new Coordinate(0, 0),
            Zone = new Zone(),
        });
    }
}