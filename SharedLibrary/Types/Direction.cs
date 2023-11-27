using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Types
{
    public class Direction
    {
        private readonly Action<int> _mcpPassedCallback;
        public Coordinate CurrentCoordinate { get; set; }
        public List<Coordinate> Waypoints { get; private set; }
        public List<int> IsMcpFlags { get; private set; }

        public bool IsCompleted { get; private set; }

        public Direction()
        {
            Waypoints = new List<Coordinate>();
            IsMcpFlags = new List<int>();
        }

        public Direction(bool isCompleted) : this()
        {
            IsCompleted = isCompleted;
        }

        public Direction(Coordinate currentCoordinate, List<int> assignedMcpIds,
            RawMapboxDirectionResponse direction, Action<int> mcpPassedCallback = null) : this()
        {
            CurrentCoordinate = currentCoordinate;
            _mcpPassedCallback = mcpPassedCallback;

            var mcpCoordinates = direction.Waypoints.Select(waypoint => new Coordinate(waypoint.Location)).ToList();
            var routeCoordinates = direction.Routes.First().Geometry.Coordinates.Select(coordinate => new Coordinate(coordinate)).ToList();

            var mcpIndex = 0;
            foreach (var routeCoordinate in routeCoordinates)
            {
                Waypoints.Add(routeCoordinate);
                if (mcpCoordinates.Any(mcpCoordinate => mcpCoordinate.IsApproximatelyEqualTo(routeCoordinate)))
                {
                    IsMcpFlags.Add(assignedMcpIds[mcpIndex]);
                    mcpIndex++;
                }
                else
                {
                    IsMcpFlags.Add(-1);
                }
            }
        }

        public Coordinate TravelBy(double distance)
        {
            var waypointsWithCurrentCoordinate = Waypoints.Prepend(CurrentCoordinate).ToList();
            var newCurrentCoordinate = Coordinate.FindDestinationCoordinate(waypointsWithCurrentCoordinate, distance);
            var passedCoordinates = Coordinate.GetTraveledCoordinates(waypointsWithCurrentCoordinate, distance).ToList();

            CurrentCoordinate = newCurrentCoordinate;
            for (var i = 0; i < passedCoordinates.Count; i++)
            {
                if (i == 0) continue;

                if (IsMcpFlags.Count == 0)
                {
                    Waypoints.Clear();
                    break;
                }

                if (IsMcpFlags[i - 1] != -1)
                {
                    Console.WriteLine($"MCP {IsMcpFlags[i - 1]} passed");
                    _mcpPassedCallback?.Invoke(IsMcpFlags[i - 1]);
                }

                Waypoints.RemoveAt(i - 1);
                IsMcpFlags.RemoveAt(i - 1);
            }

            if (Waypoints.Count <= 1) IsCompleted = true;

            return CurrentCoordinate;
        }
    }
}