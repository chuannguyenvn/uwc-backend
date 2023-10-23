using Commons.Categories;
using Repositories.Generics;
using Commons.Models;

namespace Repositories.Implementations.Accounts;

public interface IAccountRepository : IGenericRepository<Account>
{
    bool DoesUsernameExist(string username);
    Account GetByUsername(string username);
}