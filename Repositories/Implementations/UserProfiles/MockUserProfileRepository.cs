using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.UserProfiles;

public class MockUserProfileRepository : MockGenericRepository<UserProfile>, IUserProfileRepository
{
    public MockUserProfileRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }
}