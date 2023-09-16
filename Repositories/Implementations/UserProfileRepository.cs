using Repositories.Generics;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Implementations;

public class UserProfileRepository : GenericRepository<UserProfile>
{
    public UserProfileRepository(UwcDbContext context) : base(context)
    {
    }
}