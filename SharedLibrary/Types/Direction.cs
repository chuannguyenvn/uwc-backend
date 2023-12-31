using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Types
{
    public class Direction
    {
        public Coordinate CurrentCoordinate { get; set; }
        public Dictionary<int, List<Coordinate>> Legs { get; private set; }
        public List<string> Instructions { get; set; }
        public List<double> InstructionDistance { get; set; }
        public double Duration { get; set; }
        public double Distance { get; set; }

        public bool IsCompleted => Legs.Count == 0;

        public Direction()
        {
            Legs = new Dictionary<int, List<Coordinate>>();
        }

        public Direction(Coordinate currentCoordinate, List<int> assignedMcpIds,
            RawMapboxDirectionResponse direction) : this()
        {
            CurrentCoordinate = currentCoordinate;

            Instructions = new List<string>();
            InstructionDistance = new List<double>();
            for (var i = 0; i < direction.Routes.First().Legs.Count; i++)
            {
                var leg = direction.Routes.First().Legs[i];
                var legCoordinates = leg.Steps.Select(step => step.Geometry.Coordinates).SelectMany(coordinates => coordinates)
                    .Select(latLonPair => new Coordinate(latLonPair)).ToList();
                InstructionDistance.AddRange(leg.Steps.Select(step => step.Distance));
                Legs[assignedMcpIds[i]] = legCoordinates;

                for (int j = 0; j < legCoordinates.Count; j++)
                {
                    Instructions.Add(leg.Steps.First().Maneuver.Instruction);
                }
            }

            Duration = direction.Routes.First().Duration * 2;
            Distance = direction.Routes.First().Distance;
        }

        public Coordinate TravelBy(double distance)
        {
            var currentLeg = Legs.First().Value;
            var waypointsWithCurrentCoordinate = currentLeg.Prepend(CurrentCoordinate).ToList();
            var newCurrentCoordinate = Coordinate.FindDestinationCoordinate(waypointsWithCurrentCoordinate, distance);
            var passedCoordinates = Coordinate.GetTraveledCoordinates(waypointsWithCurrentCoordinate, distance).ToList();

            Legs.First().Value.RemoveRange(0, passedCoordinates.Count - 1);
            Instructions.RemoveRange(0, passedCoordinates.Count - 1);

            if (Legs.First().Value.Count == 0)
            {
                Legs.Remove(Legs.First().Key);
            }

            CurrentCoordinate = newCurrentCoordinate;
            return CurrentCoordinate;
        }
    }
}