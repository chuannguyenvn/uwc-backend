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
}