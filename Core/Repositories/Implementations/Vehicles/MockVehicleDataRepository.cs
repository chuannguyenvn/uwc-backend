using Commons.Models;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Vehicles;

public class MockVehicleDataRepository : MockGenericRepository<VehicleData>, IVehicleDataRepository
{
    public MockVehicleDataRepository(MockUwcDbContext mockUwcDbContext) : base(mockUwcDbContext)
    {
    }
}