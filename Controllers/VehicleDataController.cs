using Commons.Communications.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Services.Vehicles;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleDataController : Controller
{
    private readonly IVehicleDataService _vehicleDataService;
    
    public VehicleDataController(IVehicleDataService vehicleDataService)
    {
        _vehicleDataService = vehicleDataService;
    }
    
    [HttpPost("add")]
    public IActionResult AddNewVehicle(AddNewVehicleRequest request)
    {
        var result = _vehicleDataService.AddNewVehicle(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost("update")]
    public IActionResult UpdateVehicle(UpdateVehicleRequest request)
    {
        var result = _vehicleDataService.UpdateVehicle(request);
        return ProcessRequestResult(result);
    }
    
    [HttpPost("remove")]
    public IActionResult RemoveVehicle(RemoveVehicleRequest request)
    {
        var result = _vehicleDataService.RemoveVehicle(request);
        return ProcessRequestResult(result);
    }
}