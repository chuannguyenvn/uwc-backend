using Commons.Categories;

namespace Commons.Models;

public class VehicleData : IndexedEntity
{
    public string LicensePlate { get; set; }
    public string Model { get; set; }
    public VehicleType VehicleType { get; set; }
}