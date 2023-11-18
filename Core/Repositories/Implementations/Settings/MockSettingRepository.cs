using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Settings;

public class MockSettingRepository : MockGenericRepository<Setting>, ISettingRepository
{
    public MockSettingRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }
}