using System;

namespace Commons.Categories
{
    public enum VehicleType
    {
        SideLoader,
        FrontLoader,
        RearLoader,
    }

    public static class VehicleTypeHelper
    {
        public static string GetFriendlyString(this VehicleType vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleType.SideLoader:
                    return "Side loader";
                case VehicleType.FrontLoader:
                    return "Front loader";
                case VehicleType.RearLoader:
                    return "Rear loader";
                default:
                    throw new ArgumentOutOfRangeException(nameof(vehicleType), vehicleType, null);
            }
        }
    }
}