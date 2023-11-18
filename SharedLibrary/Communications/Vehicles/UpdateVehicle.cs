using Commons.Categories;

namespace Commons.Communications.Vehicles
{
    public class UpdateVehicle
    {
        public int VehicleId { get; set; }
        public string? NewLicensePlate { get; set; }
        public string? NewModel { get; set; }
        public VehicleType? NewVehicleType { get; set; }
    }
}