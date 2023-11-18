using Commons.Communications.Vehicles;

namespace Services.Vehicles;

public interface IVehicleDataService
{
    public RequestResult AddNewVehicle(AddNewVehicle request);
    public RequestResult UpdateVehicle(UpdateVehicle request);
    public RequestResult RemoveVehicle(RemoveVehicle request);
    public ParamRequestResult<GetAllVehicleResponse> GetAllVehicles();
}