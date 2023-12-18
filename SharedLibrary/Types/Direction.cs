using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Types
{
    public class Direction
    {
        public Coordinate CurrentCoordinate { get; set; }
        public Dictionary<int, List<Coordinate>> Legs { get; private set; }

        public bool IsCompleted => Legs.Count == 0;

        public Direction()
        {
            Legs = new Dictionary<int, List<Coordinate>>();
        }

        public Direction(Coordinate currentCoordinate, List<int> assignedMcpIds,
            RawMapboxDirectionResponse direction) : this()
        {
            CurrentCoordinate = currentCoordinate;

            for (var i = 0; i < direction.Routes.First().Legs.Count; i++)
            {
                var leg = direction.Routes.First().Legs[i];
                var legCoordinates = leg.Steps.Select(step => step.Geometry.Coordinates).SelectMany(coordinates => coordinates)
                    .Select(latLonPair => new Coordinate(latLonPair)).ToList();
                Legs[assignedMcpIds[i]] = legCoordinates;
            }
        }

        public Coordinate TravelBy(double distance)
        {
            var currentLeg = Legs.First().Value;
            var waypointsWithCurrentCoordinate = currentLeg.Prepend(CurrentCoordinate).ToList();
            var newCurrentCoordinate = Coordinate.FindDestinationCoordinate(waypointsWithCurrentCoordinate, distance);
            var passedCoordinates = Coordinate.GetTraveledCoordinates(waypointsWithCurrentCoordinate, distance).ToList();

            Legs.First().Value.RemoveRange(0, passedCoordinates.Count - 1);

            if (Legs.First().Value.Count == 0)
            {
                Legs.Remove(Legs.First().Key);
            }

            CurrentCoordinate = newCurrentCoordinate;
            return CurrentCoordinate;
        }
    }
}