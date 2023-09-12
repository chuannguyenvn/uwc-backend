using Commons.Categories;

namespace Commons.Communications.Vehicles;

public class AddNewVehicleRequest
{
    public string LicensePlate { get; set; }
    public string Model { get; set; }
    public VehicleType VehicleType { get; set; }
}