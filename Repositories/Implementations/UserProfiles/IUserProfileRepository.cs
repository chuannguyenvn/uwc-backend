using Commons.Categories;
using Commons.Models;
using Repositories.Generics;

namespace Repositories.Implementations.UserProfiles;

public interface IUserProfileRepository : IGenericRepository<UserProfile>
{
    IEnumerable<UserProfile> GetByUserRole(UserRole userRole);
}