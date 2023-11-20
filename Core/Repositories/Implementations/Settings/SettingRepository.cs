using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Settings;

public class SettingRepository : GenericRepository<Setting> , ISettingRepository
{
    public SettingRepository(UwcDbContext context) : base(context)
    {
    }
}