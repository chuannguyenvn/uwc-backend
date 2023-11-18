using System.Collections.Generic;
using Commons.Models;

namespace Commons.Communications.Vehicles
{
    public class GetAllVehicleResponse
    {
        public List<VehicleData> Vehicles { get; set; }
    }
}