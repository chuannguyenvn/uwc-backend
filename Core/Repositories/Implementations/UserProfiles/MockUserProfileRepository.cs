using Commons.Categories;
using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.UserProfiles;

public class MockUserProfileRepository : MockGenericRepository<UserProfile>, IUserProfileRepository
{
    public MockUserProfileRepository(MockUwcDbContext context) : base(context)
    {
    }

    public IEnumerable<UserProfile> GetByUserRole(UserRole userRole)
    {
        return Context.UserProfileTable.Where(up => up.UserRole == userRole);
    }

    public IEnumerable<UserProfile> GetAllWorkers()
    {
        return Context.UserProfileTable.Where(up => up.UserRole == UserRole.Driver || up.UserRole == UserRole.Cleaner);
    }
}