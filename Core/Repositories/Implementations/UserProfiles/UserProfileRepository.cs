using Commons.Categories;
using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.UserProfiles;

public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(UwcDbContext context) : base(context)
    {
    }

    public IEnumerable<UserProfile> GetByUserRole(UserRole userRole)
    {
        return Context.UserProfileTable.Where(userProfile => userProfile.UserRole == userRole);
    }
}