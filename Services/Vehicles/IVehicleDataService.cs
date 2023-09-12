using Commons.Communications.Vehicles;

namespace Services.Vehicles;

public interface IVehicleDataService
{
    public RequestResult AddNewVehicle(AddNewVehicleRequest request);
    public RequestResult UpdateVehicle(UpdateVehicleRequest request);
    public RequestResult RemoveVehicle(RemoveVehicleRequest request);
    // public RequestResult GetAllStableData(VehicleQueryParameters parameters);
    // public RequestResult GetAllVolatileData(VehicleQueryParameters parameters);
}