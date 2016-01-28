using DroneDeliverySystem.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace DroneDeliverySystem.Helpers
{
    public class MoveHelper
    {
        public static IEnumerable<Moves> GetMovesFromPath(Map map, IEnumerable<Point> shortestPath)
        {
            var previous = shortestPath.First();

            foreach (var node in shortestPath.Skip(1))
            {
                if (node.X < previous.X || (node.X == map.Width - 1 && previous.X == 0))
                    yield return Moves.Left;
                else if (node.X > previous.X || (node.X == 0 && previous.X == map.Width - 1))
                    yield return Moves.Right;
                else if (node.Y > previous.Y)
                    yield return Moves.Down;
                else if (node.Y < previous.Y)
                    yield return Moves.Up;
                else
                    Debugger.Break();

                previous = node;
            }
        }
    }
}