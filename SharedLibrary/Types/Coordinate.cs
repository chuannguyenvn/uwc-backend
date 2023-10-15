using System;
using System.Collections.Generic;
#if NET7_0
using System.ComponentModel.DataAnnotations.Schema;
#endif

namespace Commons.Types
{
#if NET7_0
    [NotMapped]
#endif
    public class Coordinate
    {
        public double Latitude;
        public double Longitude;

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate(List<double> coordinates)
        {
            Latitude = coordinates[1];
            Longitude = coordinates[0];
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.Latitude + b.Latitude, a.Longitude + b.Longitude);
        }

        public static Coordinate operator -(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.Latitude - b.Latitude, a.Longitude - b.Longitude);
        }

        public static Coordinate operator *(Coordinate a, float b)
        {
            return new Coordinate(a.Latitude * b, a.Longitude * b);
        }

        public static Coordinate operator *(Coordinate a, double b)
        {
            return new Coordinate(a.Latitude * b, a.Longitude * b);
        }

        public static Coordinate Lerp(Coordinate from, Coordinate to, float t)
        {
            return from + (to - from) * t;
        }

        public static Coordinate Lerp(Coordinate from, Coordinate to, double t)
        {
            return from + (to - from) * t;
        }

        public override string ToString()
        {
            return "Lat: " + Latitude + ". Longitude: " + Longitude;
        }
        
        public string ToStringApi()
        {
            return Longitude + "," + Latitude;
        }
    
        public double DistanceTo(Coordinate other)
        {
            return Math.Sqrt(Math.Pow(Latitude - other.Latitude, 2) + Math.Pow(Longitude - other.Longitude, 2));
        }

        public bool IsApproximatelyEqualTo(Coordinate other)
        {
            return Math.Abs(Latitude - other.Latitude) < 0.001 && Math.Abs(Longitude - other.Longitude) < 0.001;
        }
    }
}