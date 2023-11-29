﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Types
{
    public class MockCurrentRoute
    {
        private readonly Action<int> _mcpPassedCallback;
        public Coordinate CurrentCoordinate { get; private set; }
        public List<Coordinate> Waypoints { get; private set; }
        public List<int> IsMcpFlags { get; private set; }

        public bool IsCompleted => Waypoints.Count == 0;

        public MockCurrentRoute()
        {
            Waypoints = new List<Coordinate>();
            IsMcpFlags = new List<int>();
        }

        public MockCurrentRoute(Coordinate currentCoordinate, List<int> assignedMcpIds, RawMapboxDirectionResponse direction,
            Action<int> mcpPassedCallback) : this()
        {
            CurrentCoordinate = currentCoordinate;
            _mcpPassedCallback = mcpPassedCallback;

            var mcpCoordinates = direction.Waypoints.Select(waypoint => new Coordinate(waypoint.Location)).ToList();
            var routeCoordinates = direction.Routes.First().Geometry.Coordinates.Select(coordinate => new Coordinate(coordinate)).ToList();

            while (mcpCoordinates.Count > 2)
            {
                if (mcpCoordinates[0].IsApproximatelyEqualTo(routeCoordinates[0]))
                {
                    IsMcpFlags.Add(assignedMcpIds[0]);
                    mcpCoordinates.RemoveAt(0);
                    assignedMcpIds.RemoveAt(0);
                }
                else
                {
                    IsMcpFlags.Add(-1);
                }

                Waypoints.Add(routeCoordinates[0]);
                routeCoordinates.RemoveAt(0);
            }

            Waypoints.Add(routeCoordinates[0]);
            IsMcpFlags.Add(assignedMcpIds[0]);
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

            return CurrentCoordinate;
        }
    }
}