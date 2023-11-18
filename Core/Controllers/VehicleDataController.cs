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
    public IActionResult AddNewVehicle(AddNewVehicle request)
    {
        var result = _vehicleDataService.AddNewVehicle(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.VehicleData.UPDATE)]
    public IActionResult UpdateVehicle(UpdateVehicle request)
    {
        var result = _vehicleDataService.UpdateVehicle(request);
        return ProcessRequestResult(result);
    }

    [HttpPost(Endpoints.VehicleData.REMOVE)]
    public IActionResult RemoveVehicle(RemoveVehicle request)
    {
        var result = _vehicleDataService.RemoveVehicle(request);
        return ProcessRequestResult(result);
    }

    [HttpGet(Endpoints.VehicleData.GET_ALL)]
    public IActionResult GetAllVehicles()
    {
        var result = _vehicleDataService.GetAllVehicles();
        return ProcessRequestResult(result);
    }
}