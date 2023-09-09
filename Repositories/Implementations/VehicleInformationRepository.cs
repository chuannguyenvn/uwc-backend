using Commons.Models;

namespace Repositories.Implementations;

public class VehicleInformationRepository : GenericRepository<VehicleInformation>
{
    public VehicleInformationRepository(UwcDbContext context) : base(context)
    {
    }
}