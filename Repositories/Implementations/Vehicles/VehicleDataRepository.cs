using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Vehicles;

public class VehicleDataRepository : GenericRepository<VehicleData>, IVehicleDataRepository
{
    public VehicleDataRepository(UwcDbContext context) : base(context)
    {
    }
}