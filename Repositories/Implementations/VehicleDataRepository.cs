using Commons.Models;

namespace Repositories.Implementations;

public class VehicleDataRepository : GenericRepository<VehicleData>
{
    public VehicleDataRepository(UwcDbContext context) : base(context)
    {
    }
}