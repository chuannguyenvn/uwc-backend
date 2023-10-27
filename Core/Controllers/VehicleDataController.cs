using Microsoft.AspNetCore.Mvc;
using Services.Vehicles;
using Commons.Communications.Vehicles;
using Commons.Endpoints;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VehicleDataController : Controller
{
    private readonly IVehicleDataService _vehicleDataService;

    public VehicleDataController(IVehicleDataService vehicleDataService)
    {
        _vehicleDataService = vehicleDataService;
    }

    [HttpPost(Endpoints.VehicleData.ADD)]
    public IActionResult AddNewVehicle(AddNewVehicleRequest request)
    {
        var result = _vehicleDataService.AddNewVehicle(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.VehicleData.UPDATE)]
    public IActionResult UpdateVehicle(UpdateVehicleRequest request)
    {
        var result = _vehicleDataService.UpdateVehicle(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.VehicleData.REMOVE)]
    public IActionResult RemoveVehicle(RemoveVehicleRequest request)
    {
        var result = _vehicleDataService.RemoveVehicle(request);
        return ProcessRequestResult(result);
    }
}