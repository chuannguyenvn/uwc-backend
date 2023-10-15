using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Types
{
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

        public static Coordinate FindDestinationCoordinate(List<Coordinate> coordinates, double targetDistance)
        {
            if (coordinates == null || coordinates.Count < 2 || targetDistance <= 0.0)
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            double totalDistance = 0.0;

            for (int i = 0; i < coordinates.Count - 1; i++)
            {
                Coordinate start = coordinates[i];
                Coordinate end = coordinates[i + 1];
                double segmentDistance = start.DistanceTo(end);

                if (totalDistance + segmentDistance >= targetDistance)
                {
                    double remainingDistance = targetDistance - totalDistance;
                    double ratio = remainingDistance / segmentDistance;
                    double interpolatedLat = start.Latitude + (end.Latitude - start.Latitude) * ratio;
                    double interpolatedLon = start.Longitude + (end.Longitude - start.Longitude) * ratio;
                    return new Coordinate(interpolatedLat, interpolatedLon);
                }

                totalDistance += segmentDistance;
            }

            return coordinates.Last();
        }
        
        public static List<Coordinate> GetTraveledCoordinates(List<Coordinate> coordinates, double targetDistance)
        {
            if (coordinates == null || coordinates.Count < 2 || targetDistance <= 0.0)
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            List<Coordinate> traveledCoordinates = new List<Coordinate>();
            double totalDistance = 0.0;

            for (int i = 0; i < coordinates.Count - 1; i++)
            {
                Coordinate start = coordinates[i];
                Coordinate end = coordinates[i + 1];
                double segmentDistance = start.DistanceTo(end);

                if (totalDistance + segmentDistance >= targetDistance)
                {
                    double remainingDistance = targetDistance - totalDistance;
                    double ratio = remainingDistance / segmentDistance;
                    double interpolatedLat = start.Latitude + (end.Latitude - start.Latitude) * ratio;
                    double interpolatedLon = start.Longitude + (end.Longitude - start.Longitude) * ratio;
                    traveledCoordinates.Add(new Coordinate(interpolatedLat, interpolatedLon));
                    return traveledCoordinates;
                }

                totalDistance += segmentDistance;
                traveledCoordinates.Add(start);
            }

            traveledCoordinates.AddRange(coordinates.Skip(traveledCoordinates.Count));

            return traveledCoordinates;
        }
    }
}