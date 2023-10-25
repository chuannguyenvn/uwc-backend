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
        return Context.UserProfiles.Where(up => up.UserRole == userRole);
    }
}